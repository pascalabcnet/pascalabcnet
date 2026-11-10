using PascalABCCompiler.Contracts.CodeCompletion;

namespace PascalABCNet.LanguageServer.PluginApi;

/// <summary>
/// Registration surface the language server exposes to plugins.
/// </summary>
public interface ILanguageServerPluginContext
{
    /// <summary>
    /// Registers an inline (full line) completion provider. The server calls the
    /// provider outside its serialized semantic dispatcher; implementations must
    /// keep their own I/O bounded (timeouts, cancellation).
    /// </summary>
    void RegisterInlineCompletionProvider(IInlineCompletionProvider provider);
}
