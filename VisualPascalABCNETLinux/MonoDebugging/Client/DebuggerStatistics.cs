//
// DebuggerStatistics.cs
//
// Author:
//       Jeffrey Stedfast <jestedfa@microsoft.com>
//
// Copyright (c) 2019 Microsoft Corp.
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

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Mono.Debugging.Client
{
	public class DebuggerStatistics
	{
		static readonly int[] UpperTimeLimits = { 10, 50, 100, 250, 500, 1000, 1500, 2000, 5000, 10000 };

		readonly int[] buckets = new int[UpperTimeLimits.Length + 1];
		readonly object mutex = new object ();
		TimeSpan minTime = TimeSpan.MaxValue;
		TimeSpan maxTime, totalTime;

		public DebuggerStatistics(string name)
		{
			Name = name;
		}

		public double MaxTime {
			get { return maxTime.TotalMilliseconds; }
		}

		public double MinTime {
			get { return minTime.TotalMilliseconds; }
		}

		public double AverageTime {
			get {
				if (TimingsCount > 0)
					return totalTime.TotalMilliseconds / TimingsCount;

				return 0;
			}
		}

		public int TimingsCount { get; private set; }

		public int FailureCount { get; private set; }
		public string Name { get; }

		public DebuggerTimer StartTimer (string name)
		{
			return new DebuggerTimer (this, name);
		}

		int GetBucketIndex (TimeSpan duration)
		{
			var ms = (long) duration.TotalMilliseconds;

			for (var bucket = 0; bucket < UpperTimeLimits.Length; bucket++) {
				if (ms <= UpperTimeLimits[bucket])
					return bucket;
			}

			return buckets.Length - 1;
		}

		public void AddTime (TimeSpan duration)
		{
			lock (mutex) {
				if (duration > maxTime)
					maxTime = duration;

				if (duration < minTime)
					minTime = duration;

				buckets[GetBucketIndex (duration)]++;

				totalTime += duration;
				TimingsCount++;
			}
		}

		public void IncrementFailureCount ()
		{
			lock (mutex) {
				FailureCount++;
			}
		}

		public void Serialize (Dictionary<string, object> metadata)
		{
			metadata["AverageDuration"] = AverageTime;
			metadata["MaximumDuration"] = MaxTime;
			metadata["MinimumDuration"] = MinTime;
			metadata["FailureCount"] = FailureCount;
			metadata["SuccessCount"] = TimingsCount;

			for (int i = 0; i < buckets.Length; i++)
				metadata[$"Bucket{i}"] = buckets[i];
		}
	}

	public class DebuggerTimer : IDisposable
	{
		readonly DebuggerStatistics stats;
		readonly Stopwatch stopwatch;

		static readonly TraceListener traceListener = new TraceSource (nameof (DebuggerTimer)).Listeners.OfType<TraceListener> ().FirstOrDefault (l => l.Name == nameof (DebuggerTimer));
		static int traceSeq = 0;
		int traceId;

		public DebuggerTimer (DebuggerStatistics stats, string name)
		{
			stopwatch = Stopwatch.StartNew ();
			this.stats = stats;
			if (stats != null && traceListener != null) {
				traceId = Interlocked.Increment (ref traceSeq);
				traceListener.TraceEvent (null, stats.Name, TraceEventType.Start, traceId, name);
			}
		}

		/// <summary>
		/// Indicates if the debugger operation was successful. If this is false the
		/// timing will not be reported and a failure will be indicated.
		/// </summary>
		public bool Success { get; set; }

		public TimeSpan Elapsed {
			get { return stopwatch.Elapsed; }
		}

		public void Stop (bool success)
		{
			stopwatch.Stop ();
			Success = success;

			if (stats == null)
				return;
			traceListener?.TraceEvent (null, stats.Name, TraceEventType.Stop, traceId, "Stop1");

			if (success)
				stats.AddTime (stopwatch.Elapsed);
			else
				stats.IncrementFailureCount ();
		}

		public void Stop (ObjectValue val)
		{
			stopwatch.Stop ();
			if (stats == null)
				return;
			traceListener?.TraceEvent (null, stats.Name, TraceEventType.Stop, traceId, "Stop2");

			if (val.IsEvaluating || val.IsEvaluatingGroup) {
				// Do not capture timing - evaluation not finished.
			} else if (val.IsError || val.IsImplicitNotSupported || val.IsNotSupported || val.IsUnknown) {
				stats.IncrementFailureCount ();
			} else {
				// Success
				stats.AddTime (stopwatch.Elapsed);
			}
		}

		public void Dispose ()
		{
			if (stopwatch.IsRunning) {
				stopwatch.Stop ();

				if (stats == null)
					return;
				traceListener?.TraceEvent (null, stats.Name, TraceEventType.Stop, traceId, "Dispose");

				if (Success)
					stats.AddTime (stopwatch.Elapsed);
				else
					stats.IncrementFailureCount ();
			}
		}
	}
}
