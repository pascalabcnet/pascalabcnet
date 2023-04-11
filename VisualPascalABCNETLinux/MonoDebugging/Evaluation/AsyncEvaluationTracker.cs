// AsyncEvaluationTracker.cs
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
using System.Collections.Generic;
using System.Diagnostics;
using Mono.Debugging.Client;
using Mono.Debugging.Backend;

namespace Mono.Debugging.Evaluation
{
	public delegate ObjectValue ObjectEvaluatorDelegate ();
	
	/// <summary>
	/// This class can be used to generate an ObjectValue using a provided evaluation delegate.
	/// The value is initialy evaluated synchronously (blocking the caller). If no result
	/// is obtained after a short period (provided in the WaitTime property), evaluation
	/// will then be made asynchronous and the Run method will immediately return an ObjectValue
	/// with the Evaluating state.
	/// </summary>
	public class AsyncEvaluationTracker: RemoteFrameObject, IObjectValueUpdater, IDisposable
	{
		Dictionary<string, UpdateCallback> asyncCallbacks = new Dictionary<string, UpdateCallback> ();
		Dictionary<string, ObjectValue> asyncResults = new Dictionary<string, ObjectValue> ();
		int asyncCounter = 0;
		int cancelTimestamp = 0;
		TimedEvaluator runner = new TimedEvaluator ();
		
		public int WaitTime {
			get { return runner.RunTimeout; }
			set { runner.RunTimeout = value; }
		}
		
		public bool IsEvaluating {
			get { return runner.IsEvaluating; }
		}

		internal DebuggerSession Session { get; set; }

		public ObjectValue Run (string name, ObjectValueFlags flags, ObjectEvaluatorDelegate evaluator)
		{
			string id;
			int tid;
			lock (asyncCallbacks) {
				tid = asyncCounter++;
				id = tid.ToString ();
			}
			
			ObjectValue val = null;
			bool done = runner.Run (delegate {
				if (tid >= cancelTimestamp) {
					var session = Session;
					if (session == null || (flags == ObjectValueFlags.EvaluatingGroup)) {
						// Cannot report timing if session is null. If a group is being
						// evaluated then individual timings are not possible and must
						// be done elsewhere.
						val = evaluator ();
					} else {
						using (var timer = session.EvaluationStats.StartTimer (name)) {
							val = evaluator ();
							timer.Stop (val);
						}
					}
				}
			},
			delegate {
				if (tid >= cancelTimestamp)
					OnEvaluationDone (id, val);
			});
			
			if (done) {
				// 'val' may be null if the timed evaluator is disposed while evaluating
				return val ?? ObjectValue.CreateUnknown (name);
			}

			return ObjectValue.CreateEvaluating (this, new ObjectPath (id, name), flags);
		}
		
		public void Dispose ()
		{
			runner.Dispose ();
		}


		public void Stop ()
		{
			lock (asyncCallbacks) {
				cancelTimestamp = asyncCounter;
				runner.CancelAll ();
				foreach (var cb in asyncCallbacks.Values) {
					try {
						cb.UpdateValue (ObjectValue.CreateFatalError ("", "Canceled", ObjectValueFlags.None));
					} catch {
					}
				}
				asyncCallbacks.Clear ();
				asyncResults.Clear ();
			}
		}

		public void WaitForStopped ()
		{
			runner.WaitForStopped ();
		}

		void OnEvaluationDone (string id, ObjectValue val)
		{
			if (val == null)
				val = ObjectValue.CreateUnknown (null);
			UpdateCallback cb = null;
			lock (asyncCallbacks) {
				if (asyncCallbacks.TryGetValue (id, out cb)) {
					try {
						cb.UpdateValue (val);
					} catch {}
					asyncCallbacks.Remove (id);
				}
				else
					asyncResults [id] = val;
			}
		}
		
		void IObjectValueUpdater.RegisterUpdateCallbacks (UpdateCallback[] callbacks)
		{
			foreach (UpdateCallback c in callbacks) {
				lock (asyncCallbacks) {
					ObjectValue val;
					string id = c.Path[0];
					if (asyncResults.TryGetValue (id, out val)) {
						c.UpdateValue (val);
						asyncResults.Remove (id);
					} else {
						asyncCallbacks [id] = c;
					}
				}
			}
		}
	}
}
