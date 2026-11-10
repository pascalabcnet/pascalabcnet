namespace PascalABCNet.LanguageServer.PluginApi;

/// <summary>
/// Entry point of a language server plugin. The server discovers implementations
/// in assemblies named *Plugin.dll inside its plugin directories and calls
/// <see cref="Initialize"/> once at startup, before the LSP handshake.
/// </summary>
public interface ILanguageServerPlugin
{
    string Name { get; }

    void Initialize(ILanguageServerPluginContext context);
}
