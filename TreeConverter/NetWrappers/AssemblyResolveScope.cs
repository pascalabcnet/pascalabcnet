using System;
using System.Collections.Generic;
using System.IO;
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

        private Assembly OnAssemblyResolve(object obj, ResolveEventArgs args)
        {
            var requestedName = new AssemblyName(args.Name);
            if (_assembliesByName.TryGetValue(requestedName.Name, out var assembly))
                return assembly;
            var dir = PascalABCCompiler.NetHelper.NetHelper.GetAssemblyDirectory(args.RequestingAssembly);
            if (string.IsNullOrEmpty(dir))
                return null;
            string path = System.IO.Path.Combine(dir, args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll");
            if (System.IO.File.Exists(path))
            {
                try
                {
                    return PreloadAssembly(path);
                }
                catch
                {

                }

            }
            return null;
        }
    }
}