﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using Debugger.Interop.CorDebug;

namespace Debugger
{
	public class BreakpointCollection: CollectionWithEvents<Breakpoint>
	{
		public event EventHandler<CollectionItemEventArgs<Breakpoint>> Hit;
		
		protected internal void OnHit(Breakpoint item)
		{
			if (Hit != null) {
				Hit(this, new CollectionItemEventArgs<Breakpoint>(item));
			}
		}
		
		public BreakpointCollection(NDebugger debugger):base(debugger) { }
		
		internal Breakpoint this[ICorDebugBreakpoint corBreakpoint] {
			get {
				foreach (Breakpoint breakpoint in this) {
					if (breakpoint.IsOwnerOf(corBreakpoint)) {
						return breakpoint;
					}
				}
				return null;
			}
		}
		
		public new void Add(Breakpoint breakpoint)
		{
			base.Add(breakpoint);
		}
		
		public Breakpoint Add(string filename, int line)
		{
			Breakpoint breakpoint = new Breakpoint(this.Debugger, filename, null, line, 0, true);
			Add(breakpoint);
			return breakpoint;
		}
		
		public Breakpoint Add(string fileName, byte[] checkSum, int line, int column, bool enabled)
		{
			Breakpoint breakpoint = new Breakpoint(this.Debugger, fileName, checkSum, line, column, enabled);
			Add(breakpoint);
			return breakpoint;
		}
		
		protected override void OnAdded(Breakpoint breakpoint)
		{
			foreach(Process process in this.Debugger.Processes) {
				foreach(Module module in process.Modules) {
					if (breakpoint is ILBreakpoint) {
						var currentModuleTypes = module.GetNamesOfDefinedTypes();
						// set the breakpoint only if the module contains the type
						if (!currentModuleTypes.Contains(breakpoint.TypeName))
							continue;
					}
					breakpoint.SetBreakpoint(module);
				}
			}
			
			base.OnAdded(breakpoint);
		}
		
		public new void Remove(Breakpoint breakpoint)
		{
			base.Remove(breakpoint);
		}
		
		protected override void OnRemoved(Breakpoint breakpoint)
		{
			breakpoint.Deactivate();
			
			base.OnRemoved(breakpoint);
		}
		
		internal void SetInModule(Module module)
		{
			// This is in case that the client modifies the collection as a response to set breakpoint
			// NB: If client adds new breakpoint, it will be set directly as a result of his call, not here (because module is already loaded)
			List<Breakpoint> collection = new List<Breakpoint>();
			collection.AddRange(this);
			
			var currentModuleTypes = module.GetNamesOfDefinedTypes();
			foreach (Breakpoint b in collection) {
				if (b is ILBreakpoint) {
					// set the breakpoint only if the module contains the type
					if (!currentModuleTypes.Contains(b.TypeName))
						continue;
					b.SetBreakpoint(module);
				} else {
					b.SetBreakpoint(module);
				}
			}
		}
	}
}
