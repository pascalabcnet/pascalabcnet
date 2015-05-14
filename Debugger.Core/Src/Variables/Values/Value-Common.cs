// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2197 $</version>
// </file>

using System;
using Debugger.Wrappers.CorDebug;

namespace Debugger
{
	/// <summary>
	/// Provides more specific access
	/// </summary>
	public partial class Value
	{
		/// <summary> Returns true if the value is null </summary>
		public bool IsNull {
			get {
				return CorValue == null;
			}
		}
		
		/// <summary> Gets a string representation of the value </summary>
		public string AsString {
			get {
				return Cache.AsString;
			}
		}
		
		/// <summary> Gets a string representation of the value </summary>
//		public string AsString {
//			get {
//				if (this.IsNull)           return "nil";
//				if (this.Type.IsArray)     return "{" + this.Type.FullName + "}";
//				if (this.Type.IsClass)     return "{" + this.Type.FullName + "}";
//				if (this.Type.IsValueType) return "{" + this.Type.FullName + "}";
//				if (this.Type.IsPrimitive) return PrimitiveValue.ToString();
//				// Does not work well with unit tests: (variable value)
//				// if (this.Type.IsPointer)   return "0x" + this.CorValue.CastTo<ICorDebugReferenceValue>().Address.ToString("X8");
//				if (this.Type.IsPointer)   return "{" + this.Type.FullName + "}";
//				if (this.Type.IsVoid)      return "void";
//				throw new DebuggerException("Unknown type");
//			}
//		}
	}
}
