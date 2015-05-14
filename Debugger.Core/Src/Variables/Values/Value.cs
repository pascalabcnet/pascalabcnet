// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2285 $</version>
// </file>

using System;
using System.Collections.Generic;

using Debugger.Wrappers.CorDebug;

namespace Debugger
{
	/// <summary>
	/// Delegate that is used to get value. This delegate may be called at any time and should never return null.
	/// </summary>
	delegate ICorDebugValue CorValueGetter();
	
	/// <summary>
	/// Value class holds data necessaty to obtain the value of a given object
	/// even after continue. It provides functions to examine the object.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Expiration: Once value expires it can not be used anymore. 
	/// Expiration is permanet - once value expires it stays expired. 
	/// Value expires when any object specified in constructor expires 
	/// or when process exits.
	/// </para>
	/// <para>
	/// Mutation: As long as any dependecy does not mutate the last
	/// obteined value is still considered up to date. (If continue is
	/// called and internal value is neutred, new copy will be obatined)
	/// </para>
	/// </remarks>
	public partial class Value: DebuggerObject, IExpirable, IMutable
	{
		Process process;
		
		CorValueGetter corValueGetter;
		ICorDebugValue corValue;
		PauseSession   corValue_pauseSession;
		
		/// <summary> Occurs when the Value can not be used anymore </summary>
		public event EventHandler Expired;
		
		/// <summary> Occurs when the Value have potentialy changed </summary>
		public event EventHandler<ProcessEventArgs> Changed;
		
		bool isExpired = false;
		
		ValueCache cache;
		
		bool is_cnst=false;
		private class ValueCache
		{
			public PauseSession   PauseSession;
			public ICorDebugValue RawCorValue;
			public ICorDebugValue CorValue;
			public DebugType      Type;
			public string         AsString = String.Empty;
		}
		
		/// <summary>
		/// Cache stores expensive or commonly used information about the value
		/// </summary>
		unsafe ValueCache Cache {
			get {
				if (this.HasExpired) throw new CannotGetValueException("Value has expired");
				
				if (cache == null || (cache.PauseSession != process.PauseSession && !cache.RawCorValue.Is<ICorDebugHandleValue>()) || is_cnst) {
					DateTime startTime = Util.HighPrecisionTimer.Now;
					is_cnst = false;
					ValueCache newCache = new ValueCache();
					newCache.RawCorValue = corValueGetter();
					newCache.PauseSession = process.PauseSession;
					newCache.CorValue = DereferenceUnbox(newCache.RawCorValue);
					//newCache.Type = DebugType.Create(process, newCache.RawCorValue.As<ICorDebugValue2>().ExactType);
					//if (process.SelectedFunction != null)
					//newCache.Type = DebugType.Create(process, newCache.RawCorValue.As<ICorDebugValue2>().ExactType.Class,process.SelectedFunction.CorILFrame.CastTo<ICorDebugILFrame2>().EnumerateTypeParameters().ToList().ToArray());
					//else 
					newCache.Type = DebugType.Create(process, newCache.RawCorValue.As<ICorDebugValue2>().ExactType);
					cache = newCache;
					
					// AsString representation
					if (IsNull)      cache.AsString = "<null>";
					if (IsArray)     cache.AsString = "{" + this.Type.FullName + "}";
					if (IsObject)
					{
						//if (!Type.IsValueType)
						cache.AsString = "{" + this.Type.FullName + "}";
//						else
//						{
//							cache.AsString = System.Runtime.InteropServices.Marshal.ReadInt32(newCache.CorValue.Address,0).ToString();
//						}
					}
					//if (IsObject)    cache.AsString = Eval.InvokeMethod(Process, typeof(object), "ToString", this, new Value[] {}).AsString;
//					if (Type.IsByRef() && !IsPrimitive)
//						newCache.CorValue = DereferenceUnbox(newCache.CorValue);
//					else
					if (Type.IsByRef() && (PrimitiveValue == null || PrimitiveValue.ToString() == "")) cache.AsString = "{}";
					else
					if (IsPrimitive || Type.IsByRef())
						cache.AsString = PrimitiveValue != null ? PrimitiveValue.ToString() : String.Empty;
//						else
						{
							//cache.AsString = System.Runtime.InteropServices.Marshal.ReadInt32(newCache.RawCorValue.Address,0).ToString();	
//							IntPtr buf = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)newCache.RawCorValue.Size);
//							this.process.CorProcess.ReadMemory(newCache.RawCorValue.Address,newCache.RawCorValue.Size,buf);
//							int ptr = System.Runtime.InteropServices.Marshal.ReadInt32(buf);
//							cache.AsString = ptr.ToString();
//							cache.AsString = "{" + this.Type.FullName + "}";
						}
					//else
					if (Type.IsPointer) cache.AsString = "{" + this.Type.FullName + "}";
					//if (Type.IsPointer) cache.AsString = System.Runtime.InteropServices.Marshal.ReadInt32(newCache.CorValue.Address,0).ToString();
					/*if (Type.IsPointer)
					{
						IntPtr buf = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)newCache.CorValue.Size);
						this.process.CorProcess.ReadMemory(newCache.CorValue.Address,newCache.CorValue.Size,buf);
						int ptr = System.Runtime.InteropServices.Marshal.ReadInt32(buf);
						cache.AsString = ptr.ToString();
					}*/
					TimeSpan totalTime = Util.HighPrecisionTimer.Now - startTime;
					string name = this is NamedValue ? ((NamedValue)this).Name + " = " : String.Empty;
					process.TraceMessage("Obtained value: " + name + cache.AsString + " (" + totalTime.TotalMilliseconds + " ms)");
				}
				return cache;
			}
		}
		
		public Value GetPermanentReference()
		{
			ICorDebugValue corValue = this.CorValue;
			if (this.Type.IsClass) {
				corValue = this.CorObjectValue.CastTo<ICorDebugHeapValue2>().CreateHandle(CorDebugHandleType.HANDLE_STRONG).CastTo<ICorDebugValue>();
			}
			if (this.Type.IsValueType || this.Type.IsPrimitive) {
				if (!corValue.Is<ICorDebugReferenceValue>()) {
					
					// Box the value type
					if (this.Type.IsPrimitive) {
						// Get value type for the primive type
						object o = this.PrimitiveValue;
						corValue = Eval.NewObjectNoConstructor(DebugType.Create(process, null, this.Type.FullName)).RawCorValue;
						this.SetPrimitiveValue(o);
					} else {
						corValue = Eval.NewObjectNoConstructor(this.Type).RawCorValue;
					}
					// Make the reference to box permanent
					corValue = corValue.CastTo<ICorDebugReferenceValue>().Dereference().CastTo<ICorDebugHeapValue2>().CreateHandle(CorDebugHandleType.HANDLE_STRONG).CastTo<ICorDebugValue>();
					// Create new value
					Value newValue = new Value(process, new IExpirable[] {},
				                 new IMutable[] {}, delegate {return corValue;});
					// Copy the data inside the box
					newValue.CorGenericValue.RawValue = this.CorGenericValue.RawValue;
					return newValue;
				} else {
					// Make the reference to box permanent
					corValue = corValue.CastTo<ICorDebugReferenceValue>().Dereference().CastTo<ICorDebugHeapValue2>().CreateHandle(CorDebugHandleType.HANDLE_STRONG).CastTo<ICorDebugValue>();
				}
			}
			return new Value(process, new IExpirable[] {},
				                 new IMutable[] {}, delegate {return corValue;});
		}
		
		/// <summary> The process that owns the value </summary>
		[Debugger.Tests.Ignore]
		public Process Process {
			get {
				return process;
			}
		}
		
		private bool noEvaled;
		
		public bool NoEvaled
		{
			get
			{
				return noEvaled;
			}
			set
			{
				noEvaled = value;
			}
		}
		
		public void SetPrimitiveValue(object val)
		{
			if (CorType == CorElementType.STRING) {
					throw new NotSupportedException();
			} else {
					CorGenericValue.Value = val;
			}
			this.cache.AsString = val.ToString();
		}
		
		/// <summary> Returns true if the Value have expired
		/// and can not be used anymore </summary>
		public bool HasExpired {
			get {
				return isExpired;
			}
		}
		
//		public Value GetPermanentReference()
//		{
//			return new Value(this.process,new IExpirable[] {},
//				                 new IMutable[] {}, delegate { return DereferenceUnbox(this.RawCorValue).CastTo<ICorDebugHeapValue2>().CreateHandle(CorDebugHandleType.HANDLE_STRONG).CastTo<ICorDebugValue>();});
//		}

		public bool IsReferenceEqual(Value v)
		{
			if (IsNull && v.IsNull) return true;
			if (v.IsNull) return false;
			return this.CorValue.Address == v.CorValue.Address;
		}
		
		internal ICorDebugValue RawCorValue {
			get {
				return Cache.RawCorValue;
			}
		}
		
		internal ICorDebugValue CorValue {
			get {
				return Cache.CorValue;
			}
		}
 
//		internal ICorDebugValue CorValue {
//			get {
//				return this.corValue;
//			}
//		}
			
		internal CorElementType CorType {
			get {
				ICorDebugValue corValue = this.CorValue;
				if (corValue == null) {
					return (CorElementType)0;
				}
				return (CorElementType)corValue.Type;
			}
		}
		
		internal ICorDebugValue SoftReference {
			get {
				if (this.HasExpired) throw new DebuggerException("CorValue has expired");
				
				ICorDebugValue corValue = RawCorValue;
				if (corValue != null && corValue.Is<ICorDebugHandleValue>()) {
					return corValue;
				}
				corValue = DereferenceUnbox(corValue);
				if (corValue != null && corValue.Is<ICorDebugHeapValue2>()) {
					return corValue.As<ICorDebugHeapValue2>().CreateHandle(CorDebugHandleType.HANDLE_WEAK_TRACK_RESURRECTION).CastTo<ICorDebugValue>();
				} else {
					return corValue; // Value type - return value type
				}
			}
		}
		
		public Value Dereference
		{
			get
			{
				Value v = new Value(this.process,new IExpirable[] {},
				                 new IMutable[] {},delegate {return this.SoftReference;}, true);
				return v;
			}
		}
		
		private bool is_deref;
		
		internal Value(Process process,
		               IExpirable[] expireDependencies,
		               IMutable[] mutateDependencies,
		               CorValueGetter corValueGetter, bool is_deref)
		{
			this.process = process;
			
			AddExpireDependency(process);
			foreach(IExpirable exp in expireDependencies) {
				AddExpireDependency(exp);
			}
			
			foreach(IMutable mut in mutateDependencies) {
				AddMutateDependency(mut);
			}
			
			this.corValueGetter = corValueGetter;
			this.is_deref = is_deref;
		}
		
		internal Value(Process process,
		               IExpirable[] expireDependencies,
		               IMutable[] mutateDependencies,
		               CorValueGetter corValueGetter)
		{
			this.process = process;
			
			AddExpireDependency(process);
			foreach(IExpirable exp in expireDependencies) {
				AddExpireDependency(exp);
			}
			
			foreach(IMutable mut in mutateDependencies) {
				AddMutateDependency(mut);
			}
			
			this.corValueGetter = corValueGetter;
		}
		
		void AddExpireDependency(IExpirable dependency)
		{
			if (dependency.HasExpired) {
				MakeExpired();
			} else {
				dependency.Expired += delegate { MakeExpired(); };
			}
		}
		
		void MakeExpired()
		{
			if (!isExpired) {
				isExpired = true;
				OnExpired(new ValueEventArgs(this));
			}
		}
		
		void AddMutateDependency(IMutable dependency)
		{
			dependency.Changed += delegate { NotifyChange(); };
		}
		
		internal void NotifyChange()
		{
			cache = null;
			if (!isExpired) {
				OnChanged(new ValueEventArgs(this));
			}
		}
		
		/// <summary> Is called when the value changes </summary>
		protected virtual void OnChanged(ProcessEventArgs e)
		{
			if (Changed != null) {
				Changed(this, e);
			}
		}
		
		/// <summary> Is called when the value expires and can not be 
		/// used anymore </summary>
		protected virtual void OnExpired(EventArgs e)
		{
			if (Expired != null) {
				Expired(this, e);
			}
		}
		
		/// <summary> Returns the <see cref="Debugger.DebugType"/> of the value </summary>
		[Debugger.Tests.SummaryOnly]
		public DebugType Type {
			get {
				return Cache.Type;
			}
		}
		
		public void SetValue(Value newValue)
		{
			ICorDebugValue corValue = this.CorValue;
			ICorDebugValue newCorValue = newValue.CorValue;
			
			if (corValue.Is<ICorDebugReferenceValue>()) {
				if (newCorValue.Is<ICorDebugObjectValue>()) {
					ICorDebugValue box = Eval.NewObjectNoConstructor(newValue.Type).CorValue;
					newCorValue = box;
				}
				corValue.CastTo<ICorDebugReferenceValue>().SetValue(newCorValue.CastTo<ICorDebugReferenceValue>().Value);
			} else {
				corValue.CastTo<ICorDebugGenericValue>().RawValue =
					newCorValue.CastTo<ICorDebugGenericValue>().RawValue;
			}
		}
	}
	
	public class CannotGetValueException: DebuggerException
	{
		public CannotGetValueException(string message):base(message)
		{
			
		}
	}
	
	public class TooLongEvaluation : DebuggerException
	{
		public TooLongEvaluation()
		{
			
		}
	}
}
