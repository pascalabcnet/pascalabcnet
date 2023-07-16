using System;
using System.Collections.Generic;
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
        private readonly Dictionary<string, Assembly> _assemblies = new Dictionary<string, Assembly>();

        public AssemblyResolveScope(AppDomain appDomain, IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                // NOTE: assemblies with identical simple names will overwrite each other
                _assemblies[assembly.GetName().Name] = assembly;
            }

            _appDomain = appDomain;
            _appDomain.AssemblyResolve += OnAssemblyResolve;
        }

        public void Dispose()
        {
            _appDomain.AssemblyResolve -= OnAssemblyResolve;
        }

        private Assembly OnAssemblyResolve(object _, ResolveEventArgs args)
        {
            var requestedName = new AssemblyName(args.Name);
            if (_assemblies.TryGetValue(requestedName.Name, out var assembly))
                return assembly;

            return null;
        }
    }
}