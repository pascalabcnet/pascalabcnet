using PascalABCCompiler.Contracts.CodeCompletion;
using PascalABCNet.LanguageServer.PluginApi;

namespace InlineCompletionTestPlugin;

/// <summary>
/// Deterministic inline completion plugin used by the plugin infrastructure
/// smoke test. No LLM: the provider answers with canned text and echoes whether
/// it received the expected caret context, so the test also verifies the
/// document snapshot plumbing.
/// </summary>
public sealed class TestInlineCompletionPlugin : ILanguageServerPlugin
{
    public string Name => "Inline Completion Test Plugin";

    public void Initialize(ILanguageServerPluginContext context) =>
        context.RegisterInlineCompletionProvider(new TestInlineCompletionProvider());
}

internal sealed class TestInlineCompletionProvider : IInlineCompletionProvider
{
    public const string ExpectedSuggestion = "Println(stub);";

    public bool IsEnabled => true;

    public Task<string> GetCompletionAsync(CodeCompletionContext context, CancellationToken cancellationToken) =>
        Task.FromResult(context.Prefix.EndsWith("Pri", StringComparison.Ordinal)
            ? ExpectedSuggestion
            : "unexpected caret context");
}
