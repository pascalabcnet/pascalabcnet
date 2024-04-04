// EvaluationContext.cs
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
using System.Threading;

namespace Mono.Debugging.Evaluation
{
	public class TimedEvaluator
	{
		const int maxThreads = 1;

		object runningLock = new object ();
		Queue<Task> pendingTasks = new Queue<Task> ();
		ManualResetEvent newTaskEvent = new ManualResetEvent (false);
		AutoResetEvent threadNotBusyEvent = new AutoResetEvent (false);
		ManualResetEvent disposedEvent = new ManualResetEvent (false);
		List<Task> executingTasks = new List<Task> ();
		int runningThreads;
		int busyThreads;
		bool useTimeout;
		bool disposed;
		static int threadNameId;

		public TimedEvaluator () : this (true)
		{
		}

		public TimedEvaluator (bool useTimeout)
		{
			RunTimeout = 1000;
			this.useTimeout = useTimeout;
		}

		public int RunTimeout { get; set; }

		public bool IsEvaluating {
			get {
				lock (runningLock) {
					return pendingTasks.Count > 0 || busyThreads > 0;
				}
			}
		}

		/// <summary>
		/// Executes the provided evaluator. If a result is obtained before RunTimeout milliseconds,
		/// the method ends returning True.
		/// If it does not finish after RunTimeout milliseconds, the method ends retuning False, although
		/// the evaluation continues in the background. In that case, when evaluation ends, the provided
		/// delayedDoneCallback delegate is called.
		/// </summary>
		public bool Run (EvaluatorDelegate evaluator, EvaluatorDelegate delayedDoneCallback)
		{
			if (!useTimeout) {
				SafeRun (evaluator);
				return true;
			}

			Task task = new Task ();
			task.Evaluator = evaluator;
			task.FinishedCallback = delayedDoneCallback;

			lock (runningLock) {
				if (disposed)
					return false;
				if (busyThreads == runningThreads && runningThreads < maxThreads) {
					runningThreads++;
					var tr = new Thread (Runner);
					tr.Name = "Debugger evaluator " + threadNameId++;
					tr.IsBackground = true;
					tr.Start ();
				}
				pendingTasks.Enqueue (task);
				if (busyThreads == runningThreads) {
					task.TimedOut = true;
					return false;
				}
				newTaskEvent.Set ();
			}
			WaitHandle.WaitAny (new WaitHandle [] { task.RunningEvent, disposedEvent });
			if (WaitHandle.WaitAny (new WaitHandle [] { task.RunFinishedEvent, disposedEvent }, TimeSpan.FromMilliseconds (RunTimeout), false) != 0) {
				lock (task) {
					if (task.Processed) {
						return true;
					} else {
						task.TimedOut = true;
						return false;
					}
				}
			}
			return true;
		}

		void Runner ()
		{
			Task threadTask = null;

			while (!disposed) {

				if (threadTask == null) {
					lock (runningLock) {
						if (disposed) {
							runningThreads--;
							return;
						}
						if (pendingTasks.Count > 0) {
							threadTask = pendingTasks.Dequeue ();
							executingTasks.Add (threadTask);
							busyThreads++;
						} else if (busyThreads + 1 < runningThreads) {
							runningThreads--;//If we got extra non-busy threads, close this one...
							return;
						}
						if (threadTask == null) {
							newTaskEvent.Reset ();
						}
					}
					//No pending task, wait for it
					if (threadTask == null) {
						WaitHandle.WaitAny (new WaitHandle [] { newTaskEvent, disposedEvent });
						continue;
					}
				} else {
					lock (runningLock) {
						executingTasks.Remove (threadTask);
						busyThreads--;
					}
					threadNotBusyEvent.Set ();
					threadTask = null;
					continue;
				}

				threadTask.RunningEvent.Set ();
				SafeRun (threadTask.Evaluator);
				threadTask.RunFinishedEvent.Set ();
				lock (threadTask) {
					threadTask.Processed = true;
					if (threadTask.TimedOut && !disposed) {
						SafeRun (threadTask.FinishedCallback);
					}
				}
			}
		}

		public void Dispose ()
		{
			lock (runningLock) {
				disposed = true;
				CancelAll ();
				disposedEvent.Set ();
			}
		}

		public void CancelAll ()
		{
			lock (runningLock) {
				pendingTasks.Clear ();
				// If there is a task waiting the be picked by the runner,
				// set the task wait events to avoid deadlocking the caller.
				executingTasks.ForEach (t => {
					t.RunningEvent.Set ();
					t.RunFinishedEvent.Set ();
				});
			}
		}

		public void WaitForStopped ()
		{
			while (busyThreads > 0) {
				threadNotBusyEvent.WaitOne (1000);
			}
		}

		void SafeRun (EvaluatorDelegate del)
		{
			try {
				del ();
			} catch {
			}
		}

		class Task
		{
			public ManualResetEvent RunningEvent = new ManualResetEvent (false);
			public ManualResetEvent RunFinishedEvent = new ManualResetEvent (false);
			public EvaluatorDelegate Evaluator;
			public EvaluatorDelegate FinishedCallback;
			public bool TimedOut;
			public bool Processed;
		}
	}

	public delegate void EvaluatorDelegate ();
}
