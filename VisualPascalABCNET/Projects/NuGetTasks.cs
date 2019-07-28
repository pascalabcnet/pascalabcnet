// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VisualPascalABC
{
    delegate void NugetPackageInstallHandler(bool result, string[] dlls, string[] xmls);
    delegate void NugetPackageOutputHandler(string output);

    class NuGetTasks
    {
        public static void InstallPackage(string packageId, string workingDir, NugetPackageInstallHandler finishAction, NugetPackageOutputHandler outputAction)
        {
            string dir = Path.Combine(workingDir, "nuget");
            dir = Path.Combine(dir, packageId);
            Directory.CreateDirectory(dir);
            Process nugetProcess = new Process();
            nugetProcess.StartInfo.FileName = Path.Combine(PascalABCCompiler.Tools.GetExecutablePath(), "nuget.exe");
            nugetProcess.StartInfo.UseShellExecute = false;
            nugetProcess.StartInfo.Arguments = "install "+packageId+ " -NonInteractive";
            nugetProcess.StartInfo.CreateNoWindow = true;
            nugetProcess.StartInfo.RedirectStandardOutput = true;
            nugetProcess.StartInfo.RedirectStandardError = true;
            nugetProcess.StartInfo.WorkingDirectory = dir;
            nugetProcess.EnableRaisingEvents = true;
            nugetProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
            nugetProcess.Exited += new EventHandler(delegate (object o, EventArgs a) {
                string[] dlls;
                string[] xmls;
                bool result = getPackage(dir, out dlls, out xmls);
                finishAction(result, dlls, xmls);
            });
            nugetProcess.Start();
            nugetProcess.WaitForExit();
        }

        static bool getPackage(string workingDir, out string[] dlls, out string[] xmls)
        {
            dlls = null;
            xmls = null;
            List<string> dllList = new List<string>();
            List<string> xmlList = new List<string>();
            bool result = false;
            foreach (string dir in Directory.EnumerateDirectories(workingDir))
            {
                string libDir = Path.Combine(dir, "lib");
                if (Directory.Exists(libDir))
                {
                    string dotnetDir = Path.Combine(libDir, "net40");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = Path.Combine(libDir, "net45");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = Path.Combine(libDir, "net46");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = Path.Combine(libDir, "net");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = libDir;
                    if (Directory.Exists(dotnetDir))
                    {
                        string[] dllFiles = Directory.GetFiles(dotnetDir, "*.dll");
                        if (dllFiles.Length > 0)
                        {
                            dllList.AddRange(dllFiles);
                            dlls = dllFiles;
                            string[] xmlFiles = Directory.GetFiles(dotnetDir, "*.xml");
                            if (xmlFiles.Length > 0)
                                xmlList.AddRange(xmlFiles);
                            result = true;
                        }
                    }
                }
            }
            dlls = dllList.ToArray();
            xmls = xmlList.ToArray();
            return result;
        }
    }
}