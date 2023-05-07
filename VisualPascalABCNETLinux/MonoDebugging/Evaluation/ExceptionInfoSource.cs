// 
// ExceptionInfoSource.cs
//  
// Author:
//       Lluis Sanchez Gual <lluis@novell.com>
// 
// Copyright (c) 2010 Novell, Inc (http://www.novell.com)
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
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Mono.Debugging.Client;
using Mono.Debugging.Backend;

namespace Mono.Debugging.Evaluation
{
	public class ExceptionInfoSource
	{
		EvaluationContext ctx;

		public ExceptionInfoSource (EvaluationContext ctx, ValueReference exception)
		{
			Exception = exception;
			this.ctx = ctx;
		}

		public ValueReference Exception {
			get; private set;
		}

		public ObjectValue CreateObjectValue (bool withTimeout, EvaluationOptions options)
		{
			options = options.Clone ();
			options.EllipsizeStrings = false;
			options.AllowTargetInvoke = true;
			ctx = ctx.WithOptions (options);
			string type = ctx.Adapter.GetValueTypeName (ctx, Exception.ObjectValue);

			var excInstance = Exception.CreateObjectValue (withTimeout, options);
			excInstance.Name = "Instance";

			ObjectValue messageValue = null;
			ObjectValue helpLinkValue = null;
			var exceptionType = ctx.Adapter.GetValueType (ctx, Exception.Value);

			// Get the message

			if (withTimeout) {
				messageValue = ctx.Adapter.CreateObjectValueAsync ("Message", ObjectValueFlags.None, delegate {
					var mref = ctx.Adapter.GetMember (ctx, Exception, exceptionType, Exception.Value, "Message");
					if (mref != null) {
						string val = (string)mref.ObjectValue;
						return ObjectValue.CreatePrimitive (null, new ObjectPath ("Message"), "string", new EvaluationResult (val), ObjectValueFlags.Literal);
					}

					return ObjectValue.CreateUnknown ("Message");
				});
			} else {
				var mref = ctx.Adapter.GetMember (ctx, Exception, exceptionType, Exception.Value, "Message");
				if (mref != null) {
					string val = (string)mref.ObjectValue;
					messageValue = ObjectValue.CreatePrimitive (null, new ObjectPath ("Message"), "string", new EvaluationResult (val), ObjectValueFlags.Literal);
				}
			}

			if (messageValue == null)
				messageValue = ObjectValue.CreateUnknown ("Message");

			messageValue.Name = "Message";

			// Get the help link

			if (withTimeout) {
				helpLinkValue = ctx.Adapter.CreateObjectValueAsync ("HelpLink", ObjectValueFlags.None, delegate {
					var mref = ctx.Adapter.GetMember (ctx, Exception, exceptionType, Exception.Value, "HelpLink");
					if (mref != null) {
						string val = (string)mref.ObjectValue;
						return ObjectValue.CreatePrimitive (null, new ObjectPath ("HelpLink"), "string", new EvaluationResult (val), ObjectValueFlags.Literal);
					}

					return ObjectValue.CreateUnknown ("HelpLink");
				});
			} else {
				var mref = ctx.Adapter.GetMember (ctx, Exception, exceptionType, Exception.Value, "HelpLink");
				if (mref != null) {
					string val = (string)mref.ObjectValue;
					helpLinkValue = ObjectValue.CreatePrimitive (null, new ObjectPath ("HelpLink"), "string", new EvaluationResult (val), ObjectValueFlags.Literal);
				}
			}

			if (helpLinkValue == null)
				helpLinkValue = ObjectValue.CreateUnknown ("HelpLink");

			helpLinkValue.Name = "HelpLink";

			// Inner exception

			ObjectValue childExceptionValue = null;

			if (withTimeout) {
				childExceptionValue = ctx.Adapter.CreateObjectValueAsync ("InnerException", ObjectValueFlags.None, delegate {
					var inner = ctx.Adapter.GetMember (ctx, Exception, exceptionType, Exception.Value, "InnerException");
					if (inner != null && !ctx.Adapter.IsNull (ctx, inner.Value)) {
						//Console.WriteLine ("pp got child:" + type);
						var innerSource = new ExceptionInfoSource (ctx, inner);
						var res = innerSource.CreateObjectValue (false, options);
						return res;
					}

					return ObjectValue.CreateUnknown ("InnerException");
				});
			} else {
				var inner = ctx.Adapter.GetMember (ctx, Exception, exceptionType, Exception.Value, "InnerException");
				if (inner != null && !ctx.Adapter.IsNull (ctx, inner.Value)) {
					//Console.WriteLine ("pp got child:" + type);
					var innerSource = new ExceptionInfoSource (ctx, inner);
					childExceptionValue = innerSource.CreateObjectValue (false, options);
					childExceptionValue.Name = "InnerException";
				}
			}

			if (childExceptionValue == null)
				childExceptionValue = ObjectValue.CreateUnknown ("InnerException");

			// Inner exceptions in case of AgregatedException

			ObjectValue childExceptionsValue = null;
			ObjectEvaluatorDelegate getInnerExceptionsDelegate = delegate {
				var inner = ctx.Adapter.GetMember (ctx, Exception, exceptionType, Exception.Value, "InnerExceptions");
				if (inner != null && !ctx.Adapter.IsNull (ctx, inner.Value)) {
					var obj = inner.GetValue (ctx);
					var objType = ctx.Adapter.GetValueType (ctx, obj);
					var count = (int)ctx.Adapter.GetMember(ctx, null, obj, "Count").ObjectValue;
					var childrenList = new List<ObjectValue>();
					for (int i = 0; i < count; i++) {
						childrenList.Add (new ExceptionInfoSource(ctx, ctx.Adapter.GetIndexerReference(ctx, obj, objType, new object[] { ctx.Adapter.CreateValue(ctx, i) })).CreateObjectValue (withTimeout, ctx.Options));
					}
					return ObjectValue.CreateObject (null, new ObjectPath("InnerExceptions"), "", "", ObjectValueFlags.None, childrenList.ToArray ());
				}

				return ObjectValue.CreateUnknown ("InnerExceptions");
			};
			if (withTimeout) {
				childExceptionsValue = ctx.Adapter.CreateObjectValueAsync ("InnerExceptions", ObjectValueFlags.None, getInnerExceptionsDelegate);
			} else {
				childExceptionsValue = getInnerExceptionsDelegate ();
			}

			if (childExceptionsValue == null)
				childExceptionsValue = ObjectValue.CreateUnknown ("InnerExceptions");

			// Stack trace

			ObjectValue stackTraceValue;
			if (withTimeout) {
				stackTraceValue = ctx.Adapter.CreateObjectValueAsync ("StackTrace", ObjectValueFlags.None, delegate {
					var stackTrace = ctx.Adapter.GetMember (ctx, Exception, exceptionType, Exception.Value, "StackTrace");
					if (stackTrace == null)
						return ObjectValue.CreateUnknown ("StackTrace");
					return GetStackTrace (stackTrace.ObjectValue as string);
				});
			} else {
				var stackTrace = ctx.Adapter.GetMember (ctx, Exception, exceptionType, Exception.Value, "StackTrace");
				if (stackTrace == null)
					return ObjectValue.CreateUnknown ("StackTrace");
				stackTraceValue = GetStackTrace (stackTrace.ObjectValue as string);
			}

			var children = new ObjectValue [] { excInstance, messageValue, helpLinkValue, stackTraceValue, childExceptionValue, childExceptionsValue };

			return ObjectValue.CreateObject (null, new ObjectPath ("InnerException"), type, "", ObjectValueFlags.None, children);
		}

		public static ObjectValue GetStackTrace (string trace)
		{
			if (trace == null)
				return ObjectValue.CreateUnknown ("StackTrace");

			var regex = new Regex ("at (?<MethodName>.*) in (?<FileName>.*):(?<LineNumber>\\d+)(,(?<Column>\\d+))?");
			var regexLine = new Regex ("at (?<MethodName>.*) in (?<FileName>.*):line (?<LineNumber>\\d+)(,(?<Column>\\d+))?");
			var frames = new List<ObjectValue> ();

			foreach (var sframe in trace.Split ('\n')) {
				string text = sframe.Trim (' ', '\r', '\t', '"');
				string file = "";
				int column = 0;
				int line = 0;

				if (text.Length == 0)
					continue;
				//Ignore entries like "--- End of stack trace from previous location where exception was thrown ---"
				if (text.StartsWith ("---", StringComparison.Ordinal))
					continue;

				var match = regex.Match (text);
				if (match.Success) {
					text = match.Groups ["MethodName"].ToString ();
					file = match.Groups ["FileName"].ToString ();
					int.TryParse (match.Groups ["LineNumber"].ToString (), out line);
					int.TryParse (match.Groups ["Column"].ToString (), out column);
				} else {
					match = regexLine.Match (text);
					if (match.Success) {
						text = match.Groups ["MethodName"].ToString ();
						file = match.Groups ["FileName"].ToString ();
						int.TryParse (match.Groups ["LineNumber"].ToString (), out line);
						int.TryParse (match.Groups ["Column"].ToString (), out column);
					}
				}

				var fileVal = ObjectValue.CreatePrimitive (null, new ObjectPath ("File"), "", new EvaluationResult (file), ObjectValueFlags.None);
				var lineVal = ObjectValue.CreatePrimitive (null, new ObjectPath ("Line"), "", new EvaluationResult (line.ToString ()), ObjectValueFlags.None);
				var colVal = ObjectValue.CreatePrimitive (null, new ObjectPath ("Column"), "", new EvaluationResult (column.ToString ()), ObjectValueFlags.None);
				var children = new ObjectValue [] { fileVal, lineVal, colVal };

				var frame = ObjectValue.CreateObject (null, new ObjectPath (), "", new EvaluationResult (text), ObjectValueFlags.None, children);
				frames.Add (frame);
			}

			return ObjectValue.CreateArray (null, new ObjectPath ("StackTrace"), "", frames.Count, ObjectValueFlags.None, frames.ToArray ());
		}
	}
}

