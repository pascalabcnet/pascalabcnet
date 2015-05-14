// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2274 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Debugger.Wrappers.CorDebug;

namespace Debugger
{
	public enum EvalState {EvaluationScheduled, Evaluating, EvaluatedSuccessfully, EvaluatedException, EvaluatedNoResult, EvaluatedError};
	
	/// <summary>
	/// This class holds information about function evaluation.
	/// </summary>
	public class Eval: DebuggerObject
	{
		delegate void EvaluationInvoker(ICorDebugEval corEval);
		
		class EvalSetupException: System.Exception
		{
			public EvalSetupException(string msg):base(msg)
			{
			}
		}
		
		Process process;
		Value   result;
		string  description;
		EvaluationInvoker evaluationInvoker;
		
		EvalState      state = EvalState.EvaluationScheduled;
		ICorDebugEval  corEval;
		string         errorMsg;
		
		[Debugger.Tests.Ignore]
		public Process Process {
			get {
				return process;
			}
		}
		
		ICorDebugEval CorEval {
			get { return corEval; }
		}
		
		public Value Result {
			get {
				switch(this.State) {
					case EvalState.EvaluationScheduled:   throw new CannotGetValueException("Evaluation pending");
					case EvalState.Evaluating:            throw new TooLongEvaluation();
					case EvalState.EvaluatedSuccessfully: return result;
					case EvalState.EvaluatedException:    return result;
					case EvalState.EvaluatedNoResult:     return result;//throw new CannotGetValueException("No return value");
					case EvalState.EvaluatedError:        throw new CannotGetValueException(errorMsg);
					default: throw new DebuggerException("Unknown state");
				}
			}
		}
		
		public string Description {
			get {
				return description;
			}
		}
		
		public EvalState State {
			get {
				return state;
			}
		}
		
		public bool Evaluated {
			get {
				return state == EvalState.EvaluatedSuccessfully ||
				       state == EvalState.EvaluatedException ||
				       state == EvalState.EvaluatedNoResult ||
				       state == EvalState.EvaluatedError;
			}
		}
		
		Eval(Process process,
		     string description,
		     EvaluationInvoker evaluationInvoker)
		{
			this.process = process;
			this.description = description;
			this.evaluationInvoker = evaluationInvoker;
			
			process.ScheduleEval(this);
			process.Debugger.MTA2STA.AsyncCall(delegate { process.StartEvaluation(); });
		}
		
		internal bool IsCorEval(ICorDebugEval corEval)
		{
			return this.corEval == corEval;
		}
		
		/// <summary> Synchronously calls a function and returns its return value </summary>
		public static Value InvokeMethod(Process process, System.Type type, string name, Value thisValue, Value[] args)
		{
			return InvokeMethod(MethodInfo.GetFromName(process, type, name, args.Length), thisValue, args);
		}
		
		/// <summary> Synchronously calls a function and returns its return value </summary>
		public static Value InvokeMethod(MethodInfo method, Value thisValue, Value[] args)
		{
			return AsyncInvokeMethod(method, thisValue, args).EvaluateNow();
		}
		
		public static Eval AsyncInvokeMethod(MethodInfo method, Value thisValue, Value[] args)
		{
			return new Eval(
				method.Process,
				"Function call: " + method.DeclaringType.FullName + "." + method.Name,
				delegate(ICorDebugEval corEval) { StartMethodInvoke(corEval, method, thisValue, args); }
			);
		}
		
		static void StartMethodInvoke(ICorDebugEval corEval, MethodInfo method, Value thisValue, Value[] args)
		{
			List<ICorDebugValue> corArgs = new List<ICorDebugValue>();
			args = args ?? new Value[0];
			try {
				if (thisValue != null) {
					if (!(thisValue.IsObject)) {
						throw new EvalSetupException("Can not evaluate on a value which is not an object");
					}
					if (!method.DeclaringType.IsInstanceOfType(thisValue)) {
						throw new EvalSetupException("Can not evaluate because the object is not of proper type");
					}
					corArgs.Add(thisValue.SoftReference);
				}
				foreach(Value arg in args) {
					corArgs.Add(arg.SoftReference);
				}
			} catch (CannotGetValueException e) {
				throw new EvalSetupException(e.Message);
			}
			
			ICorDebugType[] genericArgs = method.DeclaringType.GenericArgumentsAsCorDebugType;
			corEval.CastTo<ICorDebugEval2>().CallParameterizedFunction(
				method.CorFunction,
				(uint)genericArgs.Length, genericArgs,
				(uint)corArgs.Count, corArgs.ToArray()
			);
			//corEval.CallFunction(method.CorFunction, (uint)corArgs.Count, corArgs.ToArray());
		}
		
		public static Value NewString(Process process, string textToCreate)
		{
			return AsyncNewString(process, textToCreate).EvaluateNow();
		}
		
		public static Value NewPrimitiveValue(Process process, object val)
		{
			return AsyncNewValue(process, val).EvaluateNow();
			
		}
		
		internal static CorElementType GetElementType(Type t)
		{
			TypeCode code = Type.GetTypeCode(t);
			switch (code)
			{
				case TypeCode.Boolean : return CorElementType.BOOLEAN;
				case TypeCode.Byte : return CorElementType.U1;
				case TypeCode.Char : return CorElementType.CHAR;
				case TypeCode.Double : return CorElementType.R8;
				case TypeCode.Int16 : return CorElementType.I2;
				case TypeCode.Int32 : return CorElementType.I4;
				case TypeCode.Int64 : return CorElementType.I8;
				case TypeCode.SByte : return CorElementType.I1;
				case TypeCode.Single : return CorElementType.R4;
				case TypeCode.UInt16 : return CorElementType.U2;
				case TypeCode.UInt32 : return CorElementType.U4;
				case TypeCode.UInt64 : return CorElementType.U8;
			}
			return CorElementType.I4;
		}
		
		public static Eval AsyncNewValue(Process process, object val)
		{
			return new Eval(
				process,
				"new value",
				delegate(ICorDebugEval corEval) {corEval.CreateValue((uint)GetElementType(val.GetType()),null);});
		}
		
		public static Eval AsyncNewString(Process process, string textToCreate)
		{
			return new Eval(
				process,
				"New string: " + textToCreate,
				delegate(ICorDebugEval corEval) { corEval.NewString(textToCreate); }
			);
		}
		
		public static Value NewObject(Process process, ICorDebugClass classToCreate)
		{
			return AsyncNewObject(process, classToCreate).EvaluateNow();
		}
		
		public static Eval AsyncNewObject(Process process, ICorDebugClass classToCreate)
		{
			return new Eval(
				process,
				"New object: " + classToCreate.Token,
				delegate(ICorDebugEval corEval) { corEval.NewObjectNoConstructor(classToCreate); }
			);
		}
		
		public static Value NewObject(Process process, DebugType debugType)
		{
			ICorDebugType ppTypeArgs=null;
			ICorDebugValue ppArgs=null;
			Eval e = new Eval(
				process,
				"New object: " + debugType.Token,
				delegate(ICorDebugEval corEval) { corEval.CastTo<ICorDebugEval2>().NewParameterizedObject
						(debugType.GetDefaultConstructor(),(uint)debugType.GenericArguments.Count,ref ppTypeArgs,0,ref ppArgs); }
			);
			return e.EvaluateNow();
		}
		
		public static Value NewObjectNoConstructor(DebugType debugType)
		{
			return AsyncNewObjectNoConstructor(debugType).EvaluateNow();
		}
		
		/*static Eval CreateEval(Process process, string description, EvalStarter evalStarter)
		{
			ICorDebugEval corEval = CreateCorEval(process);
			
			Eval newEval = new Eval(process, description, corEval);
			
			try {
				evalStarter(newEval);
			} catch (COMException e) {
				if ((uint)e.ErrorCode == 0x80131C26) {
					throw new GetValueException("Can not evaluate in optimized code");
				} else if ((uint)e.ErrorCode == 0x80131C28) {
					throw new GetValueException("Object is in wrong AppDomain");
				} else if ((uint)e.ErrorCode == 0x8013130A) {
					// Happens on getting of Sytem.Threading.Thread.ManagedThreadId; See SD2-1116
					throw new GetValueException("Function does not have IL code");
				} else if ((uint)e.ErrorCode == 0x80131C23) {
					// The operation failed because it is a GC unsafe point. (Exception from HRESULT: 0x80131C23)
					// This can probably happen when we break and the thread is in native code
					throw new GetValueException("Thread is in GC unsafe point");
				} else {
					throw;
				}
			}
			
			process.NotifyEvaluationStarted(newEval);
			process.AsyncContinue(DebuggeeStateAction.Keep);
			
			return newEval;
		}*/
		
		public static Eval AsyncNewObjectNoConstructor(DebugType debugType)
		{
			return new Eval(
				debugType.Process,
				"New object: " + debugType.FullName,
				delegate(ICorDebugEval corEval) {
					corEval.CastTo<ICorDebugEval2>().NewParameterizedObjectNoConstructor(debugType.CorType.Class, (uint)debugType.GenericArguments.Count, debugType.GenericArgumentsAsCorDebugType);
				}
			);
		}
		/// <returns>True if setup was successful</returns>
		internal bool SetupEvaluation(Thread targetThread)
		{
			process.AssertPaused();
			
			try {
				if (targetThread.IsLastFunctionNative) {
					throw new EvalSetupException("Can not evaluate because native frame is on top of stack");
				}
				if (!targetThread.IsAtSafePoint) {
					throw new EvalSetupException("Can not evaluate because thread is not at a safe point");
				}
				
				// TODO: What if this thread is not suitable?
				corEval = targetThread.CorThread.CreateEval();
				
				try {
					evaluationInvoker(corEval);
				} catch (COMException e) {
					if ((uint)e.ErrorCode == 0x80131C26) {
						throw new EvalSetupException("Can not evaluate in optimized code");
					} else if ((uint)e.ErrorCode == 0x80131C28) {
						throw new EvalSetupException("Object is in wrong AppDomain");
					} else if ((uint)e.ErrorCode == 0x8013130A) {
						// Happens on getting of Sytem.Threading.Thread.ManagedThreadId; See SD2-1116
						throw new EvalSetupException("Function does not have IL code");
					}else {
						throw;
					}
				}
				
				state = EvalState.Evaluating;
				return true;
			} catch (EvalSetupException e) {
				state = EvalState.EvaluatedError;
				errorMsg = e.Message;
				return false;
			}
		}
		
		public Value EvaluateNow()
		{
			int ticks = Environment.TickCount;
			while (!Evaluated) {
				if (process.IsPaused) { 
					process.StartEvaluation();
				}
				bool not_eval = false;
				process.WaitForPause(out not_eval);
				if (not_eval) 
				{
					this.corEval.Abort();
					break;
				}
			}
			return this.Result;
		}
		
		private static ICorDebugEval corEval2;
		
		public static Value CreateValue(Process process, object val)
		{
			corEval2 = process.SelectedThread.CorThread.CreateEval();
			Value res = new Value(process,
				                   new IExpirable[] {},
				                   new IMutable[] {},
				                   delegate { return corEval2.CreateValue((uint)GetElementType(val.GetType()),null); });
			//res.PrimitiveValue = val;
			return res;
		}
		
		internal void NotifyEvaluationComplete(bool successful) 
		{
			// Eval result should be ICorDebugHandleValue so it should survive Continue()
			
			if (corEval.Result == null) {
				state = EvalState.EvaluatedNoResult;
			} else {
				if (successful) {
					state = EvalState.EvaluatedSuccessfully;
				} else {
					state = EvalState.EvaluatedException;
				}
				result = new Value(process,
				                   new IExpirable[] {},
				                   new IMutable[] {},
				                   delegate { return corEval.Result; });
			}
		}
	}
}
