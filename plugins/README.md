# PascalABC.NET Language Server plugins

The language server loads optional feature plugins at startup. A plugin is a
separately built and installed assembly set; the server itself stays free of
feature logic such as LLM-backed completion.

## Contract

Two assemblies define the plugin surface:

- `CodeCompletionContracts` (`PascalABCCompiler.Contracts.CodeCompletion`) —
  shared editor-neutral completion contracts used by every host (Legacy IDE,
  LSP server, .NET 10 IDE): `IInlineCompletionProvider` (`IsEnabled` +
  `GetCompletionAsync(CodeCompletionContext, CancellationToken)`) and the
  `CodeCompletionContext` request DTO (file name, full text, caret offset,
  line/column with `Prefix`/`Suffix` slices);
- `PascalABCNet.LanguageServer.PluginApi` (this directory) — LSP server plugin
  mechanics: `ILanguageServerPlugin` entry point (the server finds
  implementations in assemblies whose file name ends with `Plugin.dll` and
  calls `Initialize` once, before the LSP handshake) and
  `ILanguageServerPluginContext.RegisterInlineCompletionProvider`.

Protocol surface used by the feature:

- `textDocument/inlineCompletion` (standard LSP 3.18 request) and its
  `pascalabc/inlineCompletion` mirror; result shape
  `{ "items": [ { "insertText": "..." } ] }`;
- the server advertises `experimental.inlineCompletionProvider` in the
  initialize result only when at least one plugin registered a source.

## Discovery and installation

Plugin directories are scanned at startup:

1. `<server directory>/plugins` (next to `PascalABCNet.LanguageServer.dll`);
2. every `--plugins-dir <path>` command line argument (repeatable).

A plugin is installed by dropping its assemblies (`*Plugin.dll` and its private
dependencies) into one of these directories. In VS Code that means
`<extension>/bin/net10/plugins/`; the same directory convention works for the
.NET 10 IDE host. Note: the VSIX build scripts verify the runtime kit against
its SHA-256 manifest, so install plugins after building/packaging (or keep them
outside the kit and pass `--plugins-dir`).

Plugin private dependencies are resolved next to the plugin assembly; the
contract assembly always binds to the server's own copy so interface identity
is preserved across the boundary.