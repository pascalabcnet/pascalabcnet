// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace VisualPascalABC
{
    public class EventedEventWaitHandleList
    {
        public delegate void SignalDelegate(string id);
        SignalDelegate Signal;
        Dictionary<string, ThreadObject> Threads = new Dictionary<string, ThreadObject>();

        class ThreadObject
        {
            public string id;
            public SignalDelegate Signal;
            public EventWaitHandle EventWaitHandle;
            public Thread thread;
        }

        void ThreadExecute(object obj)
        {
            ThreadObject to = (ThreadObject)obj;
            while (to.EventWaitHandle != null)
            {
                to.EventWaitHandle.WaitOne();
                if (to.EventWaitHandle != null)
                    Signal(to.id);
            }

        }

        public EventedEventWaitHandleList(SignalDelegate Signal)
        {
            this.Signal = Signal;
        }

        public void Add(string id)
        {
            ThreadObject to = new ThreadObject();
            to.thread = new Thread(new ParameterizedThreadStart(ThreadExecute));
            to.id = id;
            to.Signal = Signal;
            to.EventWaitHandle = new EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, id);
            to.thread.Start(to);
            Threads.Add(id, to);            
        }
        public void Remove(string id)
        {
            ThreadObject to = Threads[id];
            EventWaitHandle e = to.EventWaitHandle;
            to.EventWaitHandle = null;
            e.Set();
            e.Close();
            to.thread.Join();
            Threads.Remove(id);
        }
    }
}
