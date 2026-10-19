#if NET10_0
#nullable disable
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PascalABCNet.Protocol
{
    // Shared source: UI net472 and Controller net472/net10. No NuGet/IDE dependencies.
    internal sealed class RestoreActivity
    {
        private readonly object gate = new object();
        private readonly HashSet<string> operations = new HashSet<string>();
        private readonly Func<long> now;
        private long started, total, lastHeartbeat;
        public RestoreActivity(Func<long> milliseconds = null)
        {
            now = milliseconds ?? (() => (long)(Stopwatch.GetTimestamp() * 1000.0 / Stopwatch.Frequency));
        }
        public bool IsActive { get { lock (gate) return operations.Count != 0; } }
        public void Observe(string id, string stage)
        {
            if (string.IsNullOrEmpty(id)) return;
            lock (gate)
            {
                long time = now();
                if (stage == "begin")
                {
                    if (operations.Add(id) && operations.Count == 1) started = time;
                }
                else if (stage == "end")
                {
                    if (operations.Remove(id) && operations.Count == 0) total += time - started;
                }
                else if (!operations.Contains(id)) return;
                lastHeartbeat = time;
            }
        }
        private long Busy => total + (operations.Count == 0 ? 0 : now() - started);
        public Deadline BeginRequest(int milliseconds)
        {
            lock (gate) return new Deadline(this, milliseconds, now(), Busy);
        }
        internal sealed class Deadline
        {
            private readonly RestoreActivity owner;
            private readonly int limit;
            private readonly long start, busy;
            internal Deadline(RestoreActivity owner, int limit, long start, long busy)
            { this.owner = owner; this.limit = limit; this.start = start; this.busy = busy; }
            public bool Expired
            {
                get
                {
                    lock (owner.gate)
                    {
                        long elapsed = owner.now() - start;
                        // A dead server still times out: active restore must send heartbeats.
                        if (owner.operations.Count != 0 && owner.now() - owner.lastHeartbeat >= limit)
                            return elapsed >= limit;
                        return elapsed - (owner.Busy - busy) >= limit;
                    }
                }
            }
        }
    }
}
