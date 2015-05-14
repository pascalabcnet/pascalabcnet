// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2185 $</version>
// </file>

#pragma warning disable 1591

using System;
using System.Collections.Generic;
using Debugger.Interop.MetaData;
using Debugger.Wrappers.CorDebug;
using Debugger.Wrappers.CorSym;

namespace Debugger.Wrappers.MetaData
{
	public class InterfaceImplProps
	{
		public uint Method;
		public uint Class;
		public uint Interface;
	}
	
	/// <summary>
	/// Wrapper for the unmanaged metadata API
	/// </summary>
	public class MetaData: IDisposable
	{
		IMetaDataImport metaData;
		
		public MetaData(ICorDebugModule pModule)
		{
			Guid guid = new Guid("{ 0x7dac8207, 0xd3ae, 0x4c75, { 0x9b, 0x67, 0x92, 0x80, 0x1a, 0x49, 0x7d, 0x44 } }");
			metaData = (IMetaDataImport)pModule.GetMetaDataInterface(ref guid);
			ResourceManager.TrackCOMObject(metaData, typeof(IMetaDataImport));
		}
		
		public ISymUnmanagedReader GetSymReader(string fullname, string searchPath)
		{
			try {
				ISymUnmanagedBinder symBinder = new ISymUnmanagedBinder(new Debugger.Interop.CorSym.CorSymBinder_SxSClass());
				return symBinder.GetReaderForFile(metaData, fullname, searchPath);
			} catch {
				return null;
			}
		}
		
		~MetaData()
		{
			Dispose();
		}
		
		
		public void Dispose()
		{
			if (metaData != null) {
				ResourceManager.ReleaseCOMObject(metaData, typeof(IMetaDataImport));
				metaData = null;
			}
		}
		
		public TypeDefProps FindTypeDefPropsByName(string name, uint typeDef_typeRef_enclosingClass_nullable)
		{
			return GetTypeDefProps(FindTypeDefByName2(name, typeDef_typeRef_enclosingClass_nullable));
		}
		
		public IEnumerable<InterfaceImplProps> EnumInterfaceImplProps(uint typeDef)
		{
			foreach(uint token in EnumerateTokens(metaData.EnumInterfaceImpls, typeDef)) {
				yield return GetInterfaceImplProps(token);
			}
		}
		
		public InterfaceImplProps GetInterfaceImplProps(uint method)
		{
			InterfaceImplProps ret = new InterfaceImplProps();
			ret.Method = method;
			metaData.GetInterfaceImplProps(
				ret.Method,
				out ret.Class,
				out ret.Interface
			);
			return ret;
		}
		
		public int GetGenericParamCount(uint typeDef_methodDef)
		{
			int count = 0;
			foreach(uint genericParam in EnumGenericParams(typeDef_methodDef)) count++;
			return count;
		}

		public uint[] EnumGenericParams(uint typeDef_methodDef)
		{
			return EnumerateTokens(metaData.EnumGenericParams, typeDef_methodDef);
		}
		
		const int initialBufferSize = 8;
		delegate void TokenEnumerator1<T>(ref IntPtr phEnum, T parameter, uint[] token, uint maxCount, out uint fetched);
		
		uint[] EnumerateTokens<T>(TokenEnumerator1<T> tokenEnumerator, T parameter)
		{
			IntPtr enumerator = IntPtr.Zero;
			uint[] buffer = new uint[initialBufferSize];
			uint fetched;
			tokenEnumerator(ref enumerator, parameter, buffer, (uint)buffer.Length, out fetched);
			if (fetched < buffer.Length) {
				// The tokens did fit the buffer
				Array.Resize(ref buffer, (int)fetched);
			} else {
				// The tokens did not fit the buffer -> Refetch
				uint actualCount;
				metaData.CountEnum(enumerator, out actualCount);
				if (actualCount > buffer.Length) {
					buffer = new uint[actualCount];
					metaData.ResetEnum(enumerator, 0);
					tokenEnumerator(ref enumerator, parameter, buffer, (uint)buffer.Length, out fetched);
				}
			}
			metaData.CloseEnum(enumerator);
			return buffer;
		}
		
		delegate void TokenEnumerator2<T,R>(ref IntPtr phEnum, T parameter1, R parameter2, uint[] token, uint maxCount, out uint fetched);
		
		uint[] EnumerateTokens<T,R>(TokenEnumerator2<T,R> tokenEnumerator, T parameter1, R parameter2)
		{
			IntPtr enumerator = IntPtr.Zero;
			uint[] buffer = new uint[initialBufferSize];
			uint fetched;
			tokenEnumerator(ref enumerator, parameter1, parameter2, buffer, (uint)buffer.Length, out fetched);
			if (fetched < buffer.Length) {
				// The tokens did fit the buffer
				Array.Resize(ref buffer, (int)fetched);
			} else {
				// The tokens did not fit the buffer -> Refetch
				uint actualCount;
				metaData.CountEnum(enumerator, out actualCount);
				if (actualCount > buffer.Length) {
					buffer = new uint[actualCount];
					metaData.ResetEnum(enumerator, 0);
					tokenEnumerator(ref enumerator, parameter1, parameter2, buffer, (uint)buffer.Length, out fetched);
				}
			}
			metaData.CloseEnum(enumerator);
			return buffer;
		}


		public TypeDefProps GetTypeDefProps(uint typeToken)
		{
			TypeDefProps typeDefProps = new TypeDefProps();
			
			typeDefProps.Token = typeToken;
			typeDefProps.Name = 
				Util.GetString(delegate(uint pStringLenght, out uint stringLenght, System.IntPtr pString) {
					metaData.GetTypeDefProps(typeDefProps.Token,
					                         pString, pStringLenght, out stringLenght, // The string to get
					                         out typeDefProps.Flags,
					                         out typeDefProps.SuperClassToken);
				});
			
			return typeDefProps;
		}
		
		public TypeRefProps GetTypeRefProps(uint typeToken)
		{
			TypeRefProps typeRefProps = new TypeRefProps();
			
			typeRefProps.Token = typeToken;
			typeRefProps.Name =
				Util.GetString(delegate(uint pStringLenght, out uint stringLenght, System.IntPtr pString) {
					uint unused;
					metaData.GetTypeRefProps(typeRefProps.Token,
					                         out unused,
					                         pString, pStringLenght,out stringLenght // The string to get
					                         );
				});
			
			return typeRefProps;
		}
		
		public IEnumerable<FieldProps> EnumFields(uint typeToken)
		{
			IntPtr enumerator = IntPtr.Zero;
			while (true) {
				uint fieldToken;
				uint fieldsFetched;
				metaData.EnumFields(ref enumerator, typeToken, out fieldToken, 1, out fieldsFetched);
				if (fieldsFetched == 0) {
					metaData.CloseEnum(enumerator);
					break;
				}
				yield return GetFieldProps(fieldToken);
			}
		}
		
		public FieldProps GetFieldProps(uint fieldToken)
		{
			FieldProps fieldProps = new FieldProps();
			
			fieldProps.Token = fieldToken;
			fieldProps.Name =
				Util.GetString(delegate(uint pStringLenght, out uint stringLenght, System.IntPtr pString) {
					uint unused;
					IntPtr unusedPtr = IntPtr.Zero;
					metaData.GetFieldProps(fieldProps.Token,
					                       out fieldProps.ClassToken,
					                       pString, pStringLenght, out stringLenght, // The string to get
					                       out fieldProps.Flags,
					                       IntPtr.Zero,
					                       out unused,
					                       out unused,
					                       out unusedPtr,
					                       out unused);
				});
			
			return fieldProps;
		}
		
		public IEnumerable<MethodProps> EnumMethods(uint typeToken)
		{
			IntPtr enumerator = IntPtr.Zero;
			while(true) {
				uint methodToken;
				uint methodsFetched;
				metaData.EnumMethods(ref enumerator, typeToken, out methodToken, 1, out methodsFetched);
				if (methodsFetched == 0) {
					metaData.CloseEnum(enumerator);
					break;
				}
				yield return GetMethodProps(methodToken);
			}
		}
		
		public IEnumerable<PropertyProps> EnumProperties(uint typeToken)
		{
			IntPtr enumerator = IntPtr.Zero;
			while(true) {
				uint methodToken;
				uint pcProperties;
				metaData.EnumProperties(ref enumerator, typeToken, out methodToken, 1, out pcProperties);
				if (pcProperties == 0) {
					metaData.CloseEnum(enumerator);
					break;
				}
				yield return GetPropertyProps(methodToken);
			}
		}
		
		public IEnumerable<MethodProps> EnumMethodsWithName(uint typeToken, string name)
		{
			IntPtr enumerator = IntPtr.Zero;
			while(true) {
				uint methodToken;
				uint methodsFetched;
				metaData.EnumMethodsWithName(ref enumerator, typeToken, name, out methodToken, 1, out methodsFetched);
				if (methodsFetched == 0) {
					metaData.CloseEnum(enumerator);
					break;
				}
				yield return GetMethodProps(methodToken);
			}
		}
		public unsafe PropertyProps GetPropertyProps(uint propertyToken)
		{
			PropertyProps propertyProps = new PropertyProps();
			propertyProps.Token = propertyToken;
			propertyProps.Name =
				Util.GetString(delegate(uint pStringLenght, out uint stringLenght, System.IntPtr pString) {
				               	uint sigBlobSize;
				               	metaData.GetPropertyProps(propertyProps.Token,
				               	                          out propertyProps.ClassToken,
				             							  pString, pStringLenght, out stringLenght,
				             							  out propertyProps.Flags,
				             							  IntPtr.Zero,//new IntPtr(&pSigBlob),
					                        			  out sigBlobSize,
					                        			  out propertyProps.TypeKind,
					                        			  out propertyProps.DefaultValue,
					                        			  out propertyProps.DefaultValueSize,
					                        			  out propertyProps.Setter,
					                        			  out propertyProps.Getter,
					                        			  out propertyProps.OtherMethods,
                                                            2,
					                        			  out propertyProps.OtherMethodNumTokens);
				               });
			return propertyProps;
		}
		
		public unsafe MethodProps GetMethodProps(uint methodToken)
		{
			MethodProps methodProps = new MethodProps();
            try
            {
                IntPtr sigPtr = IntPtr.Zero;
                methodProps.Token = methodToken;
                methodProps.Name =
                    Util.GetString(delegate(uint pStringLenght, out uint stringLenght, System.IntPtr pString)
                {
                    uint sigBlobSize;
                    metaData.GetMethodProps(methodProps.Token,
                                            out methodProps.ClassToken,
                                            pString, pStringLenght, out stringLenght, // The string to get
                                            out methodProps.Flags,
                                            out sigPtr,//new IntPtr(&pSigBlob),
                                            out sigBlobSize,
                                            out methodProps.CodeRVA,
                                            out methodProps.ImplFlags);
                });
            }
            catch
            {
            }
			return methodProps;
		}

		
		public IEnumerable<uint> EnumParams(uint mb)
		{
			IntPtr enumerator = IntPtr.Zero;
			while(true) {
				uint token;
				uint fetched;
				metaData.EnumParams(ref enumerator, mb, out token, 1, out fetched);
				if (fetched == 0) {
					metaData.CloseEnum(enumerator);
					break;
				}
				yield return token;
			}
		}
		
		public int GetParamCount(uint methodToken)
		{
			int count = 0;
			foreach(uint param in EnumParams(methodToken)) count++;
			return count;
		}
		
		public ParamProps GetParamForMethodIndex(uint methodToken, uint parameterSequence)
		{
			uint paramToken = 0;
			metaData.GetParamForMethodIndex(methodToken, parameterSequence, ref paramToken);
			return GetParamProps(paramToken);
		}
		
		public ParamProps GetParamProps(uint paramToken)
		{
			ParamProps paramProps = new ParamProps();
			
			paramProps.Token = paramToken;
			paramProps.Name =
				Util.GetString(delegate(uint pStringLenght, out uint stringLenght, System.IntPtr pString) {
					uint unused;
					metaData.GetParamProps(paramProps.Token,
					                       out paramProps.MethodToken,
					                       out paramProps.ParameterSequence,
					                       pString, pStringLenght, out stringLenght, // The string to get
					                       out paramProps.Flags,
					                       out unused,
					                       IntPtr.Zero,
					                       out unused);
				});
			
			return paramProps;
		}
		
		public uint FindTypeDefByName2(string name, uint typeDef_typeRef_enclosingClass_nullable)
		{
			uint typeDef;
			metaData.FindTypeDefByName(name, typeDef_typeRef_enclosingClass_nullable, out typeDef);
			return typeDef;
		}
		
		public TypeDefProps FindTypeDefByName(string typeName, uint enclosingClassToken)
		{
			uint typeDefToken;
			metaData.FindTypeDefByName(typeName, enclosingClassToken, out typeDefToken);
			return GetTypeDefProps(typeDefToken);
		}
	}
}

#pragma warning restore 1591
