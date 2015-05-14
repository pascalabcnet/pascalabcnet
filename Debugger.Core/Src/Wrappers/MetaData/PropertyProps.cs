/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Pavel
 * Datum: 31.01.2008
 * Zeit: 16:39
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

#pragma warning disable 1591

using System;

namespace Debugger.Wrappers.MetaData
{
	public struct PropertyProps
	{
		public uint Token;
		public string Name;
		public uint ClassToken;
		public uint Flags;
		public uint TypeKind;
		public IntPtr DefaultValue;
		public uint DefaultValueSize;
		public uint Setter;
		public uint Getter;
		public uint OtherMethods;
		public uint OtherMethodNumTokens;
		
		public bool IsStatic {
			get {
				return (Flags & (uint)CorMethodAttr.mdStatic) != 0;
			}
		}
		
		public bool IsPublic {
			get {
				return (Flags & (uint)CorMethodAttr.mdPublic) != 0;
			}
		}
		
		public bool HasSpecialName {
			get {
				return (Flags & (uint)CorMethodAttr.mdSpecialName) != 0;
			}
		}
	}
}