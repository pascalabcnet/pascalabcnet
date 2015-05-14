// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2185 $</version>
// </file>

using System;
using System.ComponentModel;
using Debugger.Wrappers.CorDebug;

namespace Debugger
{
	// This part of the class provides support for primitive types
	// eg int, bool, string
	public partial class Value
	{
		ICorDebugGenericValue CorGenericValue {
			get {
				if (!this.Type.IsPrimitive && !this.Type.IsValueType && !this.Type.IsByRef()) throw new DebuggerException("Value is not a 'generic'");
					return CorValue.CastTo<ICorDebugGenericValue>();
				//throw new DebuggerException("Value is not a 'generic'");
			}
		}
		
		/// <summary>
		/// Returns true if the value is an primitive type.
		/// eg int, bool, string
		/// </summary>
		public bool IsPrimitive {
			get {
				return !IsNull && this.Type.IsPrimitive;
			}
		}
		
		/// <summary> Gets a value indicating whether the type is an integer type </summary>
		public bool IsInteger {
			get {
				return !IsNull && this.Type.IsInteger;
			}
		}
		
		/// <summary>
		/// Gets or sets the value of a primitive type.
		/// 
		/// If setting of a value fails, NotSupportedException is thrown.
		/// </summary>
		public object PrimitiveValue { 
			get {
				if (CorType == CorElementType.STRING) {
					return (CorValue.CastTo<ICorDebugStringValue>()).String;
				} else {
					return CorGenericValue.Value;
				}
			}
			set {
				object newValue;
				if (!is_cnst)
				{
				TypeConverter converter = TypeDescriptor.GetConverter(this.Type.ManagedType);
				try {
					newValue = converter.ConvertFrom(value);
				} catch (System.Exception e){
					//throw new System.Exception(e.Message);
					newValue = value;
					//else
					//throw new NotSupportedException("Can not convert " + value.GetType().ToString() + " to " + this.Type.ManagedType.ToString());
				}
				}
				else
				newValue = value;
				
				if (CorType == CorElementType.STRING) {
					throw new NotSupportedException();
				} else {
					CorGenericValue.Value = newValue;
				}
				this.cache.AsString = value.ToString();
				NotifyChange();
			}
		}
		
//		ICorDebugGenericValue CorGenericValue {
//			get {
//				if (!this.Type.IsPrimitive && !this.Type.IsValueType) throw new DebuggerException("Value is not a 'generic'");
//				
//				// Dereference and unbox
//				if (this.CorValue.Is<ICorDebugReferenceValue>()) {
//					return this.CorValue.CastTo<ICorDebugReferenceValue>().Dereference().CastTo<ICorDebugBoxValue>().Object.CastTo<ICorDebugGenericValue>();
//				} else {
//					return this.CorValue.CastTo<ICorDebugGenericValue>();
//				}
//			}
//		}
		
		/// <summary>
		/// Gets or sets the value of a primitive type.
		/// 
		/// If setting of a value fails, NotSupportedException is thrown.
		/// </summary>
//		public object PrimitiveValue { 
//			get {
//				if (!this.Type.IsPrimitive) throw new DebuggerException("Value is not a primitive type");
//				if (this.Type.IsString) {
//					if (this.IsNull) return null;
//					return this.CorValue.CastTo<ICorDebugReferenceValue>().Dereference().CastTo<ICorDebugStringValue>().String;
//				} else {
//					return CorGenericValue.Value;
//				}
//			}
//			set {
//				if (this.Type.IsString) {
//					throw new NotImplementedException();
//				} else {
//					if (value == null) {
//						throw new DebuggerException("Can not set primitive value to null");
//					}
//					object newValue;
//					try {
//						newValue = Convert.ChangeType(value, this.Type.ManagedType);
//					} catch {
//						throw new NotSupportedException("Can not convert " + value.GetType().ToString() + " to " + this.Type.ManagedType.ToString());
//					}
//					CorGenericValue.Value = newValue;
//				}
//			}
//		}
	}
}
