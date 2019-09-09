// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualPascalABC
{
    class FSWatcherService
    {
        Dictionary<string, FileChangeWatcher> watchers = new Dictionary<string, FileChangeWatcher>();
        public void AddWatcher(string FileName)
        {
            try
            {
                string s = FileName.ToLower();
                if (!watchers.ContainsKey(s))
                    watchers[s] = new FileChangeWatcher(FileName);
            }
            catch
            {

            }
        }

        public void DisableWatcher(string FileName)
        {
            string s = FileName.ToLower();
            FileChangeWatcher fcw = null;
            if (watchers.TryGetValue(s, out fcw))
            {
                fcw.Enabled = false;
            }
        }

        public void EnableWatcher(string FileName)
        {
            string s = FileName.ToLower();
            FileChangeWatcher fcw = null;
            if (watchers.TryGetValue(s, out fcw))
            {
                fcw.Enabled = true;
            }
        }

        public void RemoveWatcher(string FileName)
        {
            try
            {
                string s = FileName.ToLower();
                FileChangeWatcher fcw = null;
                if (watchers.TryGetValue(s, out fcw))
                {
                    fcw.Dispose();
                    watchers.Remove(s);
                }
            }
            catch
            {

            }
        }
    }
}
