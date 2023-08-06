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

        /// <remarks>Key = the resulting assembly, value = the highest version of the redirected assembly.</remarks>
        private readonly Dictionary<AssemblyName, AssemblyName> _assemblyNameOverrides =
            new Dictionary<AssemblyName, AssemblyName>();

        public ICollection<AssemblyName> BindingRedirects => _assemblyNameOverrides
            .Where(kv => kv.Key.Version != kv.Value.Version)
            .Select(kv => kv.Key)
            .ToList();

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

        private Assembly ResolveAssembly(ResolveEventArgs args)
        {
            var requestedName = new AssemblyName(args.Name);
            if (_assembliesByName.TryGetValue(requestedName.Name, out var assembly))
                return assembly;
            var dir = NetHelper.GetAssemblyDirectory(args.RequestingAssembly);
            if (string.IsNullOrEmpty(dir))
                return null;
            string path = Path.Combine(dir, args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll");
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
            return null;
        }

        private Assembly OnAssemblyResolve(object obj, ResolveEventArgs args)
        {
            var assembly = ResolveAssembly(args);
            if (assembly != null)
            {
                var resolvedName = assembly.GetName();
                var requestingName = new AssemblyName(args.Name);
                if (_assemblyNameOverrides.TryGetValue(resolvedName, out var overriddenName))
                {
                    if (overriddenName.Version < requestingName.Version)
                        _assemblyNameOverrides[resolvedName] = requestingName;
                }
                else
                {
                    _assemblyNameOverrides.Add(resolvedName, requestingName);
                }
            }

            return assembly;
        }
    }
}