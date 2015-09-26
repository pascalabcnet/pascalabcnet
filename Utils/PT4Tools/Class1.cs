using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace PT4
{
    public class Tools
    {
        string sysDir;
        Process demo, load, res;
        bool avail, disposed;
        public Tools(string PABCDir)
        {
            sysDir = PABCDir+"\\PT4";
            avail = Directory.Exists(sysDir) &&
                File.Exists(sysDir + "\\pt4pabc.dll") &&
                File.Exists(sysDir + "\\pt4demo.exe") &&
                File.Exists(sysDir + "\\pt4load.exe") &&
                File.Exists(sysDir + "\\pt4res.exe") &&
                File.Exists(sysDir + "\\results.abc");
            if (!avail) 
                return;
        }
        public bool AreAvailable
        {
            get { return avail; }
        }
        public void Demo(string startDir)
        {
            if (!avail || disposed)
                return;
            if (demo == null || demo.HasExited)
            {
                if (demo == null)
                {
                    demo = new Process();
                    demo.StartInfo.FileName = sysDir + "\\pt4demo.exe";
                }
                demo.StartInfo.Arguments = "\"" + startDir + "\" \"" + sysDir + "\"";
                demo.StartInfo.WorkingDirectory = startDir;
                demo.Start();
            }
        }
        public void Res(string startDir)
        {
            if (!avail || disposed)
                return;
            if (res == null || res.HasExited)
            {
                if (res == null)
                {
                    res = new Process();
                    res.StartInfo.FileName = sysDir + "\\pt4res.exe";
                }
                res.StartInfo.Arguments = "\"" + startDir + "\" \"" + sysDir + "\"";
                res.StartInfo.WorkingDirectory = startDir;
                res.Start();
            }
        }

        public string Load(string startDir)
        {
            if (!avail || disposed)
                return "";
            if (load == null || load.HasExited)
            {
                if (load == null)
                {
                    load = new Process();
                    load.StartInfo.FileName = sysDir + "\\pt4load.exe";
                }
                load.StartInfo.Arguments = "\"" + startDir + "\" \"" + sysDir + "\"";
                load.StartInfo.WorkingDirectory = startDir;
                load.Start();
                while (!load.HasExited)
                {
                    Application.DoEvents();
                    Thread.Sleep(50);
                    if (load == null)
                        return "";
                }
                if (!File.Exists(startDir + "\\PT4.ini"))
                    return "";
                StreamReader sr = new StreamReader(startDir + "\\PT4.ini");
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    if (s.StartsWith("File="))
                    {
                        s = s.Substring(s.IndexOf('=')+1);
                        if (File.Exists(s))
                        {
                            sr.Close();
                            return s;
                        }
                    }
                }
                sr.Close();
            }
            return "";
        }
        public void Dispose()
        {
            disposed = true;
            if (!avail)
                return;
            if (demo != null)
            {
                if (!demo.HasExited)
                    demo.Kill();
                demo.Dispose();
                demo = null;
            }
            if (res != null)
            {
                if (!res.HasExited)
                    res.Kill();
                res.Dispose();
                res = null;
            }
            if (load != null)
            {
                if (!load.HasExited)
                    load.Kill();
                load.Dispose();
                load = null;
            }
        }
        ~Tools()
        {
            if (!disposed)
                Dispose();
        }
    }
}
