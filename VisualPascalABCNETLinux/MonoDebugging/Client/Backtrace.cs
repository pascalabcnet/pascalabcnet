using System;
using System.Collections.Generic;
using Mono.Debugging.Backend;

namespace Mono.Debugging.Client
{
	[Serializable]
	public class Backtrace
	{
		IBacktrace serverBacktrace;
		int count;
		
		[NonSerialized]
		DebuggerSession session;

		List<StackFrame> frames;
		
		public Backtrace (IBacktrace serverBacktrace)
		{
			this.serverBacktrace = serverBacktrace;
			
			count = serverBacktrace.FrameCount;

			// Get first frame, which is most used(for thread location)
			if (count > 0)
				GetFrame (0, 1);
		}
		
		internal void Attach (DebuggerSession debuggerSession)
		{
			session = debuggerSession;
			serverBacktrace = session.WrapDebuggerObject (serverBacktrace);

			if (frames != null) {
				foreach (var frame in frames) {
					frame.Attach (debuggerSession);
					frame.SourceBacktrace = serverBacktrace;
				}
			}
		}

		public DebuggerSession DebuggerSession {
			get { return session; }
		}

		public int FrameCount {
			get { return count; }
		}

		public StackFrame GetFrame (int n)
		{
			return GetFrame (n, 20);
		}

		StackFrame GetFrame (int index, int fetchMultipleCount)
		{
			if (frames == null)
				frames = new List<StackFrame>();

			if (index >= frames.Count) {
				var stackFrames = serverBacktrace.GetStackFrames (frames.Count, index + fetchMultipleCount);
				foreach (var frame in stackFrames) {
					frame.SourceBacktrace = serverBacktrace;
					frame.Index = frames.Count;
					frames.Add (frame);
					frame.Attach (session);
				}
			}
			
			if (frames.Count > 0)
				return frames[Math.Min (Math.Max (0, index), frames.Count - 1)];

			return null;
		}
	}
}
