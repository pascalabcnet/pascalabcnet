using PascalABCCompiler.Contracts.CodeCompletion;
using PascalABCNet.LanguageServer.PluginApi;
using System.Reflection;
using System.Runtime.Loader;

namespace PascalABCNet.LanguageServer;

/// <summary>
/// Discovers, loads and hosts language server plugins. Plugin assemblies are
/// named *Plugin.dll and live in the server's plugin directory (next to the
/// server assembly) or in directories passed with --plugins-dir.
/// </summary>
internal sealed class PluginHost : ILanguageServerPluginContext
{
    private readonly List<IInlineCompletionProvider> _inlineCompletionProviders = new();

    public IReadOnlyList<IInlineCompletionProvider> InlineCompletionProviders => _inlineCompletionProviders;

    public bool HasInlineCompletionProvider => _inlineCompletionProviders.Count > 0;

    void ILanguageServerPluginContext.RegisterInlineCompletionProvider(IInlineCompletionProvider provider) =>
        _inlineCompletionProviders.Add(provider);

    public static PluginHost Load(IEnumerable<string> pluginDirectories, TextWriter log)
    {
        var host = new PluginHost();
        foreach (var directory in pluginDirectories)
        {
            if (!Directory.Exists(directory))
                continue;

            foreach (var pluginPath in Directory.EnumerateFiles(directory, "*Plugin.dll"))
            {
                try
                {
                    var loadContext = new PluginLoadContext(pluginPath);
                    var assembly = loadContext.LoadFromAssemblyPath(Path.GetFullPath(pluginPath));
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.IsAbstract || !typeof(ILanguageServerPlugin).IsAssignableFrom(type))
                            continue;

                        var plugin = (ILanguageServerPlugin)Activator.CreateInstance(type)!;
                        plugin.Initialize(host);
                        log.WriteLine($"Loaded LSP plugin '{plugin.Name}' from {pluginPath}");
                    }
                }
                catch (Exception exception)
                {
                    log.WriteLine($"Failed to load LSP plugin {pluginPath}: {exception.Message}");
                }
            }
        }

        return host;
    }

    /// <summary>
    /// Loads a plugin and its private dependencies in an isolated context
    /// </summary>
    private sealed class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public PluginLoadContext(string pluginPath)
            : base(Path.GetFileNameWithoutExtension(pluginPath), isCollectible: false)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            // Host first: any assembly the server already provides (shared
            // contracts, common dependencies) must resolve to a single copy so
            // interface identity survives the plugin boundary.
            // Only what the host does not have is taken from the plugin's own files.
            try
            {
                var hostAssembly = Default.LoadFromAssemblyName(assemblyName);
                if (hostAssembly is not null)
                    return hostAssembly;
            }
            catch (Exception)
            {
                // Not provided by the host.
            }

            var path = _resolver.ResolveAssemblyToPath(assemblyName);
            return path is null ? null : LoadFromAssemblyPath(path);
        }
    }
}
