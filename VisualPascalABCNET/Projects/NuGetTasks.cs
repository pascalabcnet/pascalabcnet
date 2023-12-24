// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VisualPascalABC
{
    delegate void NugetPackageInstallHandler(bool result, string[] dlls, string[] xmls, string[] ndlls);
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
            nugetProcess.StartInfo.CreateNoWindow = false;
            nugetProcess.StartInfo.RedirectStandardOutput = false;
            nugetProcess.StartInfo.RedirectStandardError = false;
            nugetProcess.StartInfo.WorkingDirectory = dir;
            nugetProcess.EnableRaisingEvents = true;
            //nugetProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
            nugetProcess.Exited += new EventHandler(delegate (object o, EventArgs a) {
                string[] dlls;
                string[] xmls;
                string[] ndlls;
                bool result = getPackage(dir, out dlls, out xmls, out ndlls);
                finishAction(result, dlls, xmls, ndlls);
            });
            nugetProcess.Start();
            nugetProcess.WaitForExit();
        }

        static string getDotnetDir(string startDir)
        {
            // 481 48 472 471 47 462 461 46 452 451 45 4
            var s = "481 48 472 471 47 462 461 46 452 451 45 4";
            var ss = s.Split().Reverse().Select(d => "net" + d).ToList();
            ss.Add("netstandard2.0");
            ss.Add("net");

            string Dir = "";
            foreach (var dd in ss)
            {
                Dir = Path.Combine(startDir, dd);
                if (Directory.Exists(Dir))
                    break;
            }

            /*string Dir = Path.Combine(startDir, "net40");
            if (!Directory.Exists(Dir))
                Dir = Path.Combine(startDir, "net45");
            if (!Directory.Exists(Dir))
                Dir = Path.Combine(startDir, "net46");
            if (!Directory.Exists(Dir))
                Dir = Path.Combine(startDir, "netstandard2.0");
            if (!Directory.Exists(Dir))
                Dir = Path.Combine(startDir, "net");*/

            if (!Directory.Exists(Dir))
                Dir = startDir;
            return Dir;
        }

        static bool getPackage(string workingDir, out string[] dlls, out string[] xmls, out string[] ndlls)
        {
            dlls = null;
            xmls = null;
            ndlls = null;
            List<string> dllList = new List<string>();
            List<string> xmlList = new List<string>();
            List<string> nativedllList = new List<string>();
            bool result = false;
            foreach (string dir in Directory.EnumerateDirectories(workingDir))
            {
                string libDir = Path.Combine(dir, "lib");
                if (Directory.Exists(libDir))
                {
                    string dotnetDir = getDotnetDir(libDir);

                    /*string dotnetDir = Path.Combine(libDir, "net40");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = Path.Combine(libDir, "net45");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = Path.Combine(libDir, "net46");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = Path.Combine(libDir, "netstandard2.0");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = Path.Combine(libDir, "net");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = libDir;*/

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
                // А теперь разбираемся с native dlls. Они находятся в папке runtimes\win-x64\nativeassets\такое же окончание как в dotnetdir
                string runtimesDir = Path.Combine(dir, "runtimes", "win-x64", "nativeassets");
                if (!Directory.Exists(runtimesDir))
                    runtimesDir = Path.Combine(dir, "runtimes", "win-x64", "native");
                if (Directory.Exists(runtimesDir))
                {
                    /*string dotnetDir = Path.Combine(runtimesDir, "net40");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = Path.Combine(runtimesDir, "net45");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = Path.Combine(runtimesDir, "net46");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = Path.Combine(runtimesDir, "netstandard2.0");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = Path.Combine(runtimesDir, "net");
                    if (!Directory.Exists(dotnetDir))
                        dotnetDir = runtimesDir;
                    */
                    string dotnetDir = getDotnetDir(runtimesDir);

                    if (Directory.Exists(dotnetDir))
                    {
                        string[] nativedllFiles = Directory.GetFiles(dotnetDir, "*.dll");
                        if (nativedllFiles.Length > 0)
                        {
                            nativedllList.AddRange(nativedllFiles);
                        }
                    }
                }
            }
            dlls = dllList.ToArray();
            xmls = xmlList.ToArray();
            ndlls = nativedllList.ToArray();
            return result;
        }
    }
}