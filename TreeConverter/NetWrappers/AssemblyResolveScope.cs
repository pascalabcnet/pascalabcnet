using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PascalABCCompiler.NetHelper
{
    /// <summary>
    /// Will force the application domain to load passed assemblies when requested, while ignoring the assembly
    /// versions. Handles <see cref="AppDomain.AssemblyResolve"/> until disposed.
    /// </summary>
    public class AssemblyResolveScope : IDisposable
    {
        private readonly AppDomain _appDomain;
        private readonly Dictionary<string, Assembly> _assembliesByName = new Dictionary<string, Assembly>();
        private readonly Dictionary<string, Assembly> _assembliesByPath = new Dictionary<string, Assembly>();

        public ICollection<AssemblyName> CalculateBindingRedirects()
        {
            var redirectedAssemblies = new HashSet<string>();
            foreach (var assembly in _assembliesByName.Values)
            {
                foreach (var referenceName in assembly.GetReferencedAssemblies())
                {
                    var shortName = referenceName.Name;
                    if (_assembliesByName.TryGetValue(shortName, out var referenceAssembly)
                        && referenceAssembly.GetName().Version != referenceName.Version)
                    {
                        redirectedAssemblies.Add(shortName);
                    }
                }
            }

            return redirectedAssemblies.Select(a => _assembliesByName[a].GetName()).ToList();
        }

        public AssemblyResolveScope(AppDomain appDomain)
        {
            _appDomain = appDomain;
            _appDomain.AssemblyResolve += OnAssemblyResolve;
        }

        public Assembly PreloadAssembly(string assemblyFilePath)
        {
            // Convert to normalized form (slashes, name case, possible relative paths):
            assemblyFilePath = Path.GetFullPath(assemblyFilePath);

            if (_assembliesByPath.TryGetValue(assemblyFilePath, out var assembly))
                return assembly;
            
            assembly = NetHelper.LoadAssembly(assemblyFilePath);
            _assembliesByName[assembly.GetName().Name] = assembly;
            _assembliesByPath[assemblyFilePath] = assembly;
            return assembly;
        }

        public void Dispose()
        {
            _appDomain.AssemblyResolve -= OnAssemblyResolve;
        }

        static string standartAssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(string)).ManifestModule.FullyQualifiedName);

        private string GetStandardAssemblyPath(string name)
        {
            name = name.Replace("%GAC%\\", "");
            string ttn = System.IO.Path.GetFileNameWithoutExtension(name);
            string tn = Path.Combine(standartAssemblyPath, name);
            if (File.Exists(tn))
                return tn;
            if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX)
            {
                string windir = Path.Combine(Environment.GetEnvironmentVariable("windir"), "Microsoft.NET");
                tn = windir + @"\assembly\GAC_MSIL\";
                tn += ttn + "\\";
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(tn);
                if (!di.Exists)
                {
                    tn = windir + @"\assembly\GAC_64\";
                    tn += ttn + "\\";
                    di = new System.IO.DirectoryInfo(tn);
                    if (!di.Exists)
                    {
                        tn = windir + @"\assembly\GAC_32\";
                        tn += ttn + "\\";
                        di = new System.IO.DirectoryInfo(tn);
                        if (!di.Exists)
                        {
                            tn = windir + @"\assembly\GAC\";
                            tn += ttn + "\\";
                            di = new System.IO.DirectoryInfo(tn);
                            if (!di.Exists)
                            {
                                windir = Environment.GetEnvironmentVariable("windir");
                                tn = windir + @"\assembly\GAC_MSIL\";
                                tn += ttn + "\\";
                                di = new System.IO.DirectoryInfo(tn);
                                if (!di.Exists)
                                {
                                    tn = windir + @"\assembly\GAC_64\";
                                    tn += ttn + "\\";
                                    di = new System.IO.DirectoryInfo(tn);
                                    if (!di.Exists)
                                    {
                                        tn = windir + @"\assembly\GAC_32\";
                                        tn += ttn + "\\";
                                        di = new System.IO.DirectoryInfo(tn);
                                        if (!di.Exists)
                                        {
                                            tn = windir + @"\assembly\GAC\";
                                            tn += ttn + "\\";
                                            di = new System.IO.DirectoryInfo(tn);
                                            if (!di.Exists)
                                            {
                                                return null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                System.IO.DirectoryInfo[] diarr = di.GetDirectories();
                tn = Path.Combine((diarr[0]).FullName, name);
            }
            else
            {
                string gac_path = "/usr/lib/mono/4.0/gac";
                DirectoryInfo di = new DirectoryInfo(Path.Combine(gac_path, ttn));
                if (di.Exists)
                {
                    System.IO.DirectoryInfo[] diarr = di.GetDirectories();
                    tn = Path.Combine((diarr[diarr.Length - 1]).FullName, name);
                }
                else
                    return null;

            }
            return tn;

        }

        public List<string> missingAssemblies = new List<string>();

        private Assembly OnAssemblyResolve(object obj, ResolveEventArgs args)
        {
            if (args.RequestingAssembly == null) //fix Linux/Mono IDE missworking after loading a huge pas source such as GraphABC.pas or GraphABCHelper.pas
                return null;
            var requestedName = new AssemblyName(args.Name);
            if (_assembliesByName.TryGetValue(requestedName.Name, out var assembly))
                return assembly;
            var dir = NetHelper.GetAssemblyDirectory(args.RequestingAssembly);
            if (string.IsNullOrEmpty(dir))
                return null;
            string fileName = args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
            string path = Path.Combine(dir, fileName);
            if (!File.Exists(path))
            {
                string assmPath = GetStandardAssemblyPath(fileName);
                if (assmPath != null)
                    path = assmPath;
            }
            if (File.Exists(path))
            {
                try
                {
                    return PreloadAssembly(path);
                }
                catch
                {

                }

            }
            if (!missingAssemblies.Contains(fileName))
                missingAssemblies.Add(fileName);
            return null;
        }
    }
}
