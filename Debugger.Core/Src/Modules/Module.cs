﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2274 $</version>
// </file>

using System;
using Debugger.Wrappers.CorDebug;
using Debugger.Wrappers.CorSym;
using Debugger.Wrappers.MetaData;

namespace Debugger
{
	public class Module: DebuggerObject, IDisposable
	{
		Process process;
		
		bool   unloaded = false;
		string fullPath;
		
		int orderOfLoading = 0;
		ICorDebugModule corModule;
		ISymUnmanagedReader symReader;
		MetaData metaData;
		
		[Debugger.Tests.Ignore]
		public Process Process {
			get {
				return process;
			}
		}
		
		internal MetaData MetaData {
			get {
				return metaData;
			}
		}
		
		public bool Unloaded {
			get {
				return unloaded;
			}
		}
		
		public ISymUnmanagedReader SymReader {
			get {
				return symReader;
			}
		}
		
		public ICorDebugModule CorModule {
			get {
				return corModule;
			}
		}
		
		internal uint AppDomainID {
			get {
				return this.CorModule.Assembly.AppDomain.ID;
			}
		}
		
		public ulong BaseAdress { 
			get {
				return corModule.BaseAddress;
			} 
		}
		
		public bool IsDynamic { 
			get {
				return corModule.IsDynamic == 1;
			} 
		}
		
		public bool IsInMemory { 
			get {
				return corModule.IsInMemory == 1;
			} 
		}
		
		public string FullPath { 
			get {
				return fullPath;
			} 
		}
		
		public string Filename {
			get {
				if (IsDynamic || IsInMemory) return String.Empty;
				return System.IO.Path.GetFileName(FullPath);
			}
		}
		
		public string DirectoryName {
			get {
				if (IsDynamic || IsInMemory) return String.Empty;
				return System.IO.Path.GetDirectoryName(FullPath);
			}
		}
		
		public bool SymbolsLoaded { 
			get {
				return symReader != null;
			} 
		}
		
		public int OrderOfLoading { 
			get {
				return orderOfLoading;
			}
			set {
				orderOfLoading = value;
			}
		}
		
		public bool JMCStatus {
			set {
				uint unused = 0;
				if (corModule.Is<ICorDebugModule2>()) { // Is the debuggee .NET 2.0?
					(corModule.CastTo<ICorDebugModule2>()).SetJMCStatus(value?1:0, 0, ref unused);
				}
			}
		}
		
		internal Module(Process process, ICorDebugModule pModule)
		{
			this.process = process;
			
			corModule = pModule;
			
			metaData = new MetaData(pModule);
			
			fullPath = pModule.Name;
			
			symReader = metaData.GetSymReader(fullPath, null);
			
			JMCStatus = SymbolsLoaded;
		}
		
		public void ApplyChanges(byte[] metadata, byte[] il)
		{
			if (corModule.Is<ICorDebugModule2>()) { // Is the debuggee .NET 2.0?
				(corModule.CastTo<ICorDebugModule2>()).ApplyChanges((uint)metadata.Length, metadata, (uint)il.Length, il);
			}
		}
		
		public void Dispose()
		{
			metaData.Dispose();
			if (symReader != null) {
				symReader.As<ISymUnmanagedDispose>().Destroy();
			}
			
			unloaded = true;
		}
		
		public override string ToString()
		{
			return string.Format("{0}", this.Filename);
		}
	}
}
